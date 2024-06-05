using System.Collections.Concurrent;
using WindyFarm.Gin.Network.Protocol.NetwortSetup;
using WindyFarm.Gin.Network.Utils;
using WindyFarm.Gin.ServerLog;

namespace WindyFarm.Gin.Network.Protocol
{
    public class MessagePool
    {
        public static readonly MessagePool Instance = new();
        private readonly ConcurrentDictionary<MessageTag, Message> Pool;
        private MessagePool() 
        { 
            Pool = new ConcurrentDictionary<MessageTag, Message>();
            Init();
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

        public Message? ParseMessage(String json)
        {
            Dummy? dummy = JsonHelper.ParseObject<Dummy>(json);

            if (dummy == null) return null;
            try
            {
                Message? message = Get(dummy.Tag);
                message?.Decode(json);
                return message;
            }
            catch (Exception ex)
            {
                GinLogger.Error(ex);
            }

            return null;
        }
    }
}
