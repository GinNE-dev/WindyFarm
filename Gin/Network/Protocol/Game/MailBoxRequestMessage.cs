using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Network.Protocol.Game
{
    public class MailBoxRequestMessage : Message
    {
        public override MessageTag Tag => MessageTag.MailBoxRequest;

        public override bool Execute(IMessageHandler handler) => handler.handleMailBoxRequest(this);
        protected override void DecodeJson(string json) { }
    }
}
