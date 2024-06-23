using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network.Protocol;

namespace WindyFarm.Gin.Network.Protocol.Game
{
    public class InventoryRequestMessage : Message
    {
        public override MessageTag Tag => MessageTag.InventoryDataRequest;

        public override bool Execute(IMessageHandler handler)
        {
            return handler.handleInventoryRequest(this);
        }

        protected override void DecodeJson(string json)
        {

        }
    }
}
