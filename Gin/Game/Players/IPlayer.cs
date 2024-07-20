using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Data;
using WindyFarm.Gin.Network;
using WindyFarm.Gin.Network.Protocol;

namespace WindyFarm.Gin.Game.Players
{
    public interface IPlayer
    {
        public string SessionId {  get; }
        public Guid Id { get;}
        public string DisplayName { get; }
        public int Gold { get; }
        public int Diamond { get; }
        public int Level { get; }
        public int Exp { get; }
        public string Gender { get; }
        public int MaxInventory { get; }
        public string Email { get; }
        public double PositionX { get; }
        public double PositionY { get; }
        public double PositionZ { get; }
        public int MapId { get; }

        public int LevelUpExp { get; }
        public void GainExp(int amount);
        public bool SendMessageAsync(IMessage message);
        public void MoveTo(Vector3 pos, Vector3 dir);
    }
}
