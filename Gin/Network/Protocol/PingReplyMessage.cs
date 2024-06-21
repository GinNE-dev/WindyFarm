using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Network.Protocol
{
    public class PingReplyMessage : Message
    {
        public override MessageTag Tag => MessageTag.PingReply;

        public override bool Execute(IMessageHandler handler)
        {
            return handler.handlePingReply(this);
        }

        protected override void DecodeJson(string json)
        {
            
        }
    }
}
