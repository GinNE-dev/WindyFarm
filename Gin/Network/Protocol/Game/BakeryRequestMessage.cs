using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Network.Protocol.Game
{
    public class BakeryRequestMessage : Message
    {
        public override MessageTag Tag => MessageTag.BakeryRequest;

        public override bool Execute(IMessageHandler handler) => handler.handleBakeryRequest(this);

        protected override void DecodeJson(string json) { }
    }
}
