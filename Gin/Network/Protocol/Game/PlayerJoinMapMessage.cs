using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network.Utils;

namespace WindyFarm.Gin.Network.Protocol.Game
{
    public class PlayerJoinMapMessage : Message
    {
        public override MessageTag Tag => MessageTag.PlayerJoinMap;
        public Guid PlayerId { get; set; }

        public string DisplayName { get; set; } = string.Empty;

        public int Diamond { get; set; }

        public int Gold { get; set; }

        public int Level { get; set; } = 1;

        public int Exp { get; set; }

        public int Energy { get; set; }

        public int EnergyCapacity { get; set; }

        public int LevelUpExp { get; set; }

        public string Gender { get; set; } = "Male"!;
        public int MapId {  get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Direction { get; set; }

        public override bool Execute(IMessageHandler handler) => handler.handlePlayerJoinMap(this);

        protected override void DecodeJson(string json)
        {
            PlayerJoinMapMessage? message = JsonHelper.ParseObject<PlayerJoinMapMessage>(json);
            if(message is not null)
            {
                MapId = message.MapId;
                PlayerId = message.PlayerId;
                DisplayName = message.DisplayName;
                Diamond = message.Diamond;
                Gold = message.Gold;
                Level = message.Level;
                Exp = message.Exp;
                Energy = message.Energy;
                EnergyCapacity = message.EnergyCapacity;
                LevelUpExp = message.LevelUpExp;
                Gender = message.Gender;
                Position = message.Position;
                Direction = message.Direction;
            }
        }
    }
}
