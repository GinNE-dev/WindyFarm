using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Data;
using WindyFarm.Gin.Game.Items;
using WindyFarm.Gin.Network;
using WindyFarm.Gin.Network.Protocol;
using WindyFarm.Gin.SystemLog;

namespace WindyFarm.Gin.Game.Players
{
    public class Player : IPlayer
    {
        private readonly int MAX_LEVEL = 100;
        public Guid Id => _playerData.Id;
        public string SessionId => _session.SessionId;
        public string DisplayName => _playerData.DisplayName;
        public int Gold => _playerData.Gold;
        public int Diamond => _playerData.Diamond;
        public int Level => _playerData.Level;
        public int Exp => _playerData.Exp;
        public string Gender => _playerData.Gender;
        public int MaxInventory => _playerData.MaxInventory;
        public string Email => _playerData.AccountId;
        public double PositionX => _playerData.PositionX;
        public double PositionY => _playerData.PositionY;
        public double PositionZ => _playerData.PositionZ;
        public int MapId => _playerData.MapId;
        public readonly Inventory Inventory;
        public readonly PlayerDat _playerData;
        private readonly Server _server;
        private readonly Session _session;
        private readonly WindyFarmDatabaseContext _dbContext;
        public Player(WindyFarmDatabaseContext dbContext, Server server, Session session, PlayerDat playerData)
        {
            _dbContext = dbContext;
            _playerData = playerData;
            _server = server;
            _session = session;

            Inventory = new Inventory(this, _dbContext);
        }


        public int LevelUpExp => (int)Math.Round(86.95 * Math.Pow(1.15, Level));

        public void GainExp(int amount)
        {
            int newExp = Exp + amount;
            while (newExp >= LevelUpExp)
            {
                newExp -= LevelUpExp;
                if(Level < MAX_LEVEL) _playerData.Level += 1;
            }

            _playerData.Exp = newExp;
        }

        public bool SendMessageAsync(IMessage message)
        {
            return _session.SendMessageAsync(message);
        }

        public void MoveTo(double x, double y, double z)
        {
            _playerData.PositionX = x;
            _playerData.PositionY = y;
            _playerData.PositionZ = z;
        }
    }
}
