using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Game.Players;
using WindyFarm.Gin.Network.Protocol.Game;

namespace WindyFarm.Gin.Game.Maps
{
    public abstract class PrivateMap : Map
    {
        protected override void OnPlayerJoinAsNew(Player player)
        {
            var enteredFrom = MapManager.Instance.Get(player.MapId);
            if (enteredFrom is not null)
            {
                Vector3 spawnPoint = EnteredSpawnPoints.GetValueOrDefault(enteredFrom.GetType(), Vector3.Zero);
                player.MoveTo(spawnPoint, Vector3.Zero);
            }
            
            player.ChangeMap(Id);
            EnterMapResultMessage m1 = new() { EnteredMapId = Id, Accepted = true };
            player.SendMessageAsync(m1);
        }

        protected override void OnPlayerLeave(Player player) { }

        protected override void OnPlayerRejoin(Player player) 
        {
            
        }
    }
}
