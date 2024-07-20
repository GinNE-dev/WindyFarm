using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network.Utils;

namespace WindyFarm.Gin.Network.Protocol.Game
{
    public class EnterMapMessage : Message
    {
        public override MessageTag Tag => MessageTag.EnterMap;
        public int MapId { get; set; }
        public override bool Execute(IMessageHandler handler) => handler.handleEnterMap(this);

        protected override void DecodeJson(string json)
        {
            EnterMapMessage? m = JsonHelper.ParseObject<EnterMapMessage>(json);
            if(m is not null)
            {
                MapId = m.MapId;
            }
        }
    }
}
