using WindyFarm.Gin.Network.Utils;

namespace WindyFarm.Gin.Network.Protocol.NetwortSetup
{
    public class SendKeyMessage : Message
    {
        public override MessageTag Tag { get;} = MessageTag.KeySend;
        public byte[]? Key { get; set; }
        public byte[]? IV { get; set; }

        protected override void DecodeJson(string json)
        {
            SendKeyMessage? message = JsonHelper.ParseObject<SendKeyMessage>(json);
            if (message != null)
            {
                Key = message.Key;
                IV = message.IV;
            }
        }

        public override bool Execute(IMessageHandler handler)
        {
            return handler.handleKeyReceived(this);
        }
    }
}
