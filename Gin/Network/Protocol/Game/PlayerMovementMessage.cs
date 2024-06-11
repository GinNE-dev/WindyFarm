using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network.Utils;

namespace WindyFarm.Gin.Network.Protocol.Game
{
    public class PlayerMovementMessage : Message
    {
        public override MessageTag Tag => MessageTag.PlayerMovement;
        public double PositionX { get; set; }
        public double PositionY { get; set; }
        public double PositionZ { get; set; }

        public override bool Execute(IMessageHandler handler)
        {
            return handler.handlePlayerMovement(this);
        }

        protected override void DecodeJson(string json)
        {
            PlayerMovementMessage? msg = JsonHelper.ParseObject<PlayerMovementMessage>(json);
            if (msg != null)
            {
                PositionX = msg.PositionX;
                PositionY = msg.PositionY;
                PositionZ = msg.PositionZ;
            }
        }
    }
}
