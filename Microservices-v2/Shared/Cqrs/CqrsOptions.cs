namespace Industria4.Cqrs
{
    /// <summary>
    ///     Options containing connection strings and service bus settings
    /// </summary>
    public class CqrsOptions
    {
        public string RabbitMqConnectionString { get; set; }

        public string SqlServerConnectionString { get; set; }

        public int ServiceBusWorkers { get; set; } = 10;
    }
}