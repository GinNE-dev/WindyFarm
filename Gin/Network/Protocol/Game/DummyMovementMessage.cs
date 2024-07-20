using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network.Utils;

namespace WindyFarm.Gin.Network.Protocol.Game
{
    public class DummyMovementMessage : Message
    {
        public override MessageTag Tag => MessageTag.DummyMovement;
        public Guid PlayerId { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Direction { get; set; }

        public override bool Execute(IMessageHandler handler) => handler.handleDummyMovement(this);

        protected override void DecodeJson(string json)
        {
            DummyMovementMessage? message = JsonHelper.ParseObject<DummyMovementMessage>(json);
            if(message is not null)
            {
                PlayerId = message.PlayerId;
                Position = message.Position;
                Direction = message.Direction;
            }
        }
    }
}
