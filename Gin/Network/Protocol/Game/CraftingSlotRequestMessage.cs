using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Network.Protocol.Game
{
    public class CraftingSlotRequestMessage : Message
    {
        public override MessageTag Tag => MessageTag.CraftingRequest;

        public override bool Execute(IMessageHandler handler) => handler.handleCraftingSlotRequest(this);

        protected override void DecodeJson(string json) { }
    }
}
