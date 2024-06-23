using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Network.Protocol.Game
{
    public class FarmlandRequestMessage : Message
    {
        public override MessageTag Tag => MessageTag.FarmlandDataRequest;

        public override bool Execute(IMessageHandler handler) => handler.handleFarmlandRequest(this);

        protected override void DecodeJson(string json){ }
    }
}
