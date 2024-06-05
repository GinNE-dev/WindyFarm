using System;
using System.Collections.Concurrent;
using WindyFarm.Gin.Network.Protocol.NetwortSetup;
using WindyFarm.Gin.Network.Utils;
using WindyFarm.Gin.SystemLog;

namespace WindyFarm.Gin.Network.Protocol
{
    public class MessagePool
    {
        public static readonly MessagePool Instance = new();
        private readonly ConcurrentDictionary<MessageTag, Message> Pool;
        private MessagePool() 
        { 
            Pool = new ConcurrentDictionary<MessageTag, Message>();
        }

        public void Init()
        {
            Register(new RequestKeyMessage());
            Register(new SendKeyMessage());
            Register(new ConfirmKeyMessage());
            Register(new ConnectionResultMessage());
            Register(new TextMessage());
        }

        public bool Register(Message? message)
        {
            if(message == null) return false;
            return Pool.TryAdd(message.Tag, message);
        }

        public Message? Get(MessageTag tag)
        {
            Pool.TryGetValue(tag, out Message? message);
            return message?.Clone();
        }

        public Message? ParseMessage(string json)
        {
            Dummy? dummy = JsonHelper.ParseObject<Dummy>(json);

            if (dummy == null)
            {
                GinLogger.Warning($"[{GetType()}]: Can't parse Message tag!");
                return null;
            }
            Message? message = Get(dummy.Tag);
            if (message == null)
            {
                GinLogger.Warning($"[{GetType()}]: Message not found in pool, maybe Message with tag '{dummy.Tag}' not registered!");
                return null;
            }

            message.Decode(json);
            return message;
        }
    }
}
