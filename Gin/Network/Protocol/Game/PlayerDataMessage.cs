using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network.Utils;

namespace WindyFarm.Gin.Network.Protocol.Game
{
    public class PlayerDataMessage : Message
    {
        public override MessageTag Tag => MessageTag.PlayerDataResponse;
        public Guid Id { get; set; }

        public string DisplayName { get; set; } = string.Empty;

        public int Diamond { get; set; }

        public int Gold { get; set; }

        public int Level { get; set; } = 1;

        public int Exp { get; set; }

        public string Gender { get; set; } = "Male"!;

        public int MaxInventory { get; set; } = 25;

        public double PositionX { get; set; }

        public double PositionY { get; set; }

        public double PositionZ { get; set; }

        public int MapId { get; set; } = 0;

        public string AccountId { get; set; } = string.Empty!;

        public override bool Execute(IMessageHandler handler)
        {
            return handler.handlePlayerDataResponse(this);
        }

        protected override void DecodeJson(string json)
        {
            PlayerDataMessage? msg = JsonHelper.ParseObject<PlayerDataMessage>(json);
            if(msg != null)
            {
                Id = msg.Id;
                DisplayName = msg.DisplayName;
                Gold = msg.Gold;
                Diamond = msg.Diamond;
                Level = msg.Level;
                Exp = msg.Exp;
                Gender = msg.Gender;
                MaxInventory = msg.MaxInventory;
                PositionX = msg.PositionX;
                PositionY = msg.PositionY;
                PositionZ = msg.PositionZ;
                MapId = msg.MapId;
                AccountId = msg.AccountId;
            }
        }
    }
}
