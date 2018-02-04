using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.Extensions.Logging;
using Muuvis.Catalog.Cqrs.Events;
using Muuvis.Cqrs.Events;
using Muuvis.Taste.Cqrs.Events;
using Rebus.Bus;
using Rebus.Handlers;
using Rebus.Messages;
using Rebus.Pipeline;
using Rebus.Sagas;

namespace Muuvis.Catalog.Cqrs.Handlers
{
    public class AddMovieSaga : Saga<AddMovieSaga.AddMovieSagaData>, IAmInitiatedBy<MovieAddedEvent>, IAmInitiatedBy<SuggestionEvaluatedEvent>, IHandleMessages<CompletionTimeoutEvent<string>>
    {
        private readonly IBus _bus;
        private readonly ILogger<AddMovieSaga> _logger;

        public AddMovieSaga(IBus bus, ILogger<AddMovieSaga> logger)
        {
            _bus = bus;
            _logger = logger;
        }

        public class AddMovieSagaData : SagaData
        {
            public string MovieId { get; set; }

            public List<Type> ReceivedEvents { get; } = new List<Type>();

            public string Originator { get; set; }

            public string Reply { get; set; }
        }

        protected override void CorrelateMessages(ICorrelationConfig<AddMovieSagaData> config)
        {
            config.Correlate<MovieAddedEvent>(e => e.MovieId, s => s.MovieId);
            config.Correlate<SuggestionEvaluatedEvent>(e => e.MovieId, s => s.MovieId);
            config.Correlate<CompletionTimeoutEvent<string>>(e => e.Data, s => s.MovieId);
        }

        public Task Handle(MovieAddedEvent message)
        {
            return EvaluateCompletedAsync(message);
        }

        public Task Handle(SuggestionEvaluatedEvent message)
        {
            return EvaluateCompletedAsync(message);
        }

        private async Task EvaluateCompletedAsync<T>(T message)
        {
            if (IsNew)
            {
                dynamic d = message;
                await _bus.DeferLocal(TimeSpan.FromSeconds(3), new CompletionTimeoutEvent<string>(d.MovieId));

                Message rawMessage = MessageContext.Current.Message;
                Data.Originator = rawMessage.Headers[Headers.ReturnAddress];
                Data.Reply = rawMessage.Headers[Headers.CorrelationId];
            }

            _logger.LogInformation("Received message {message}", message);

            if (!Data.ReceivedEvents.Contains(typeof(T)))
            {
                Data.ReceivedEvents.Add(typeof(T));
            }
            
            if (Data.ReceivedEvents.Count == 2)
            {
                _logger.LogInformation("Saga completed");

                // Workaround for sending reply
                //IMessageContext messageContext = MessageContext.Current;
                //string messageId = messageContext.TransportMessage.GetMessageId();
                //string returnAddress = messageContext.TransportMessage.Headers[Headers.ReturnAddress];
                //messageContext.TransportMessage.Headers[Headers.ReturnAddress] = Data.Originator;
                //messageContext.TransportMessage.Headers[Headers.MessageId] = Data.Reply;
                //await _bus.Reply(Data.MovieId);
                //messageContext.TransportMessage.Headers[Headers.ReturnAddress] = returnAddress;
                //messageContext.TransportMessage.Headers[Headers.MessageId] = messageId;

                MarkAsComplete();
            }
        }

        public Task Handle(CompletionTimeoutEvent<string> message)
        {
            MarkAsComplete();

            _logger.LogWarning("Saga timeout");

            return Task.CompletedTask;
        }
    }
}
