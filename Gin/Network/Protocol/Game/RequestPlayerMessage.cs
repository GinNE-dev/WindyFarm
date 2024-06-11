using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network.Utils;

namespace WindyFarm.Gin.Network.Protocol.Game
{
    public class RequestPlayerMessage : Message
    {
        public override MessageTag Tag => MessageTag.PlayerDataRequest;

        public override bool Execute(IMessageHandler handler)
        {
            return handler.handlePlayerDataRequest(this);
        }

        protected override void DecodeJson(string json)
        {
        }
    }
}
