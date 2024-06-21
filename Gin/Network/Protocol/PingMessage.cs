using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.SystemLog;

namespace WindyFarm.Gin.Network.Protocol
{
    public class PingMessage : Message
    {
        public override MessageTag Tag => MessageTag.Ping;

        public override bool Execute(IMessageHandler handler)
        {
            return handler.handlePing(this);
        }

        protected override void DecodeJson(string json)
        {
            
        }
    }
}
