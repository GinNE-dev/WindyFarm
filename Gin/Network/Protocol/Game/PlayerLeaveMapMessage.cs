using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network.Utils;

namespace WindyFarm.Gin.Network.Protocol.Game
{
    public class PlayerLeaveMapMessage : Message
    {
        public override MessageTag Tag => MessageTag.PlayerLeaveMap;
        public Guid PlayerId { get; set; }
        public int MapId { get; set; }

        public override bool Execute(IMessageHandler handler) => handler.handlePlayerLeaveMap(this);

        protected override void DecodeJson(string json)
        {
            PlayerLeaveMapMessage? message = JsonHelper.ParseObject<PlayerLeaveMapMessage>(json);
            if(message is not null)
            {
                PlayerId = message.PlayerId;
                MapId = message.MapId;
            }
        }
    }
}
