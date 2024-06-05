using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network.Utils;

namespace WindyFarm.Gin.Network.Protocol.NetwortSetup
{
    public class RequestKeyMessage : Message
    {
        public override MessageTag Tag { get; } = MessageTag.KeyRequest;

        public override bool Execute(IMessageHandler handler)
        {
            return handler.handleKeyRequest(this);
        }

        protected override void DecodeJson(string json)
        {

        }

        protected override string EncodeJson()
        {
            return JsonHelper.ParseString(this);
        }
    }
}
