using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Game.Players;

namespace WindyFarm.Gin.Game.Maps
{
    public abstract class Map
    {
        public abstract int Id { get;}
        public abstract string Name { get;}
        //public abstract Vector3 SpawnPoint { get;}
        //public abstract Dictionary<int, Vector3> Portals { get; }
        public abstract Dictionary<Type, Vector3> EnteredSpawnPoints { get; }
        public void PlayerJoin(Player player) 
        {
            if (player.MapId.Equals(this.Id))
            {
                OnPlayerRejoin(player);
            }
            else
            {
                OnPlayerJoinAsNew(player);
            }
            
        }

        protected abstract void OnPlayerJoinAsNew(Player player);

        protected abstract void OnPlayerRejoin(Player player);

        public void PlayerLeave(Player player)
        {
            OnPlayerLeave(player);
        }

        protected abstract void OnPlayerLeave(Player player);
    }
}
