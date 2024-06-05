using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Network;

namespace WindyFarm.Gin.Core
{
    public class PlayerManager
    {
        private static PlayerManager? _instance;
        public ConcurrentDictionary<string, Session> Players { get; private set; }
        private PlayerManager()
        {
            Players = new ConcurrentDictionary<string, Session>();
        }

        public static PlayerManager Instance
        {
            get
            {
                _instance ??= new PlayerManager();
                return _instance;
            }
        }

        public void AddPlayer(Session user)
        {
            if (user == null) return;
            Players.TryAdd(user.SessionId, user);
        }
    }
}
