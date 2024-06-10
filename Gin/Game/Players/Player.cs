using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Database.Models;
using WindyFarm.Gin.Network;

namespace WindyFarm.Gin.Game.Players
{
    internal class Player : IPlayer
    {
        private readonly int MAX_LEVEL = 100;
        public string SessionId => _session.SessionId;
        public string DisplayName => _playerData.DisplayName;
        public int Gold => _playerData.Gold;
        public int Diamond => _playerData.Diamond;
        public int Level => _playerData.Level;
        public int Exp => _playerData.Exp;
        private readonly PlayerDat _playerData;
        private readonly Server _server;
        private readonly Session _session;
        public Player(Server server, Session session, PlayerDat playerData)
        {
            _playerData = playerData;
            _server = server;
            _session = session;
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
    }
}
