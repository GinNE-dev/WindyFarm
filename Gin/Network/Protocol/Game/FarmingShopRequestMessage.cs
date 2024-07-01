using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Network.Protocol.Game
{
    public class FarmingShopRequestMessage : Message
    {
        public override MessageTag Tag => MessageTag.FarmingShopRequest;

        public override bool Execute(IMessageHandler handler) => handler.handleFarmingShopRequest(this);

        protected override void DecodeJson(string json) { }
    }
}
