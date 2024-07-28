using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Players
{
    public class OnlinePlayerManager
    {
        private static OnlinePlayerManager? _instance;
        public static OnlinePlayerManager Instance 
        {
            get 
            {
                _instance ??= new OnlinePlayerManager();
                return _instance; 
            }
        }

        private readonly ConcurrentDictionary<Guid, Player> _players;
        private OnlinePlayerManager()
        {
            _players = new();
        }

        public void AddPlayer(Player player)
        {
            if (player is null) return;
            _players.TryAdd(player.Id, player);
        }

        public Player? GetPlayer(Guid playerId)
        {
            _players.TryGetValue(playerId, out Player? foundPlayer);
            return foundPlayer;
        }

        public Player? RemovePlayer(Guid playerId)
        {
            _players.TryRemove(playerId, out Player? removedPlayer);

            return removedPlayer;
        }

        public Player? RemovePlayer(Player player)
        {
            if(player is null) return null;
            _players.TryRemove(player.Id, out Player? removedPlayer);
            return removedPlayer;
        }

        public bool IsOnline(Guid playerId) => _players.ContainsKey(playerId);
    }
}
