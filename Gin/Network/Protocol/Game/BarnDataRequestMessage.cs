using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Network.Protocol.Game
{
    public class BarnDataRequestMessage : Message
    {
        public override MessageTag Tag => MessageTag.BarnRequest;

        public override bool Execute(IMessageHandler handler) => handler.handleBarnDataResquest(this);

        protected override void DecodeJson(string json) { }
    }
}
