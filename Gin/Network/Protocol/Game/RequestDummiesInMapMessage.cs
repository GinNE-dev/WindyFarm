using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network.Utils;

namespace WindyFarm.Gin.Network.Protocol.Game
{
    public class RequestDummiesInMapMessage : Message
    {
        public override MessageTag Tag => MessageTag.RequestDummiesInMap;
        public int MapId { get; set; }

        public override bool Execute(IMessageHandler handler) => handler.handleRequestDummiesInMap(this);

        protected override void DecodeJson(string json)
        {
            RequestDummiesInMapMessage? m = JsonHelper.ParseObject<RequestDummiesInMapMessage>(json);
            if (m is not null)
            {
                MapId = m.MapId;
            }
        }
    }
}
