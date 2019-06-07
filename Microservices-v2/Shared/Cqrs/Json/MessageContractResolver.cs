using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Industria4.Cqrs.Messaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Industria4.Cqrs.Json
{
    /// <summary>
    /// Custom contract resolver for IMessage
    /// </summary>
    public class MessageContractResolver : DefaultContractResolver
    {
        static MessageContractResolver()
        {
            MessageIdProperty = typeof(MessageBase).GetRuntimeProperty(nameof(MessageBase.MessageId));
        }

        private static readonly PropertyInfo MessageIdProperty;

        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            List<MemberInfo> list = base.GetSerializableMembers(objectType);
            // Avoid serialization of MessageId
            list.Remove(MessageIdProperty);

            return list;
        }
    }
}
