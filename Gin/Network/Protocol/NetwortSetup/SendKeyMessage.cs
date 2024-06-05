using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network.Utils;
using WindyFarm.Gin.ServerLog;

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

        protected override string EncodeJson()
        {
            return JsonHelper.ParseString(this);
        }

        public override bool Execute(IMessageHandler handler)
        {
            return handler.handleKeyReceived(this);
        }
    }
}
