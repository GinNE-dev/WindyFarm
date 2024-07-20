using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Game.Players;
using WindyFarm.Gin.Network.Protocol;
using WindyFarm.Gin.Network.Protocol.Game;
using WindyFarm.Gin.SystemLog;

namespace WindyFarm.Gin.Game.Maps
{
    public abstract class PublicMap : Map
    {
        public readonly int MAX_PLAYER = 32;
        protected ConcurrentDictionary<Guid, Player> PlayerInMap = new();

        public bool HasSlot() => PlayerInMap.Count < MAX_PLAYER;
        protected override void OnPlayerJoinAsNew(Player player)
        {
            GinLogger.Print($"Player[id:{player.Id}] joined Map[id:[{Id}] as new");
            if (!HasSlot())
            {
                EnterMapResultMessage m = new() { EnteredMapId = Id, Accepted = false, ExtraMessage = "Full" };
                player.SendMessageAsync(m);
                return;
            }

            var enteredFrom = MapManager.Instance.Get(player.MapId);
            if (enteredFrom is not null)
            {
                Vector3 spawnPoint = EnteredSpawnPoints.GetValueOrDefault(enteredFrom.GetType(), Vector3.Zero);
                player.MoveTo(spawnPoint, Vector3.Zero);
            }

            LetPlayerJoin(player);
        }

        private void LetPlayerJoin(Player player)
        {
            PlayerInMap.TryAdd(player.Id, player);

            RegisterDisconnect(player);
            RegisterPlayerMove(player);

            player.ChangeMap(Id);
            EnterMapResultMessage m1 = new() { EnteredMapId = Id, Accepted = true };
            player.SendMessage(m1);
            NotifyJoinToOthers(player);
        }

        private void RegisterPlayerMove(Player player)
        {
            player.OnMove += (pos, dir) =>
            {
                Message? m = MessagePool.Instance.Get(MessageTag.DummyMovement);
                if(m is  null)
                {
                    GinLogger.Debug("Can't find DummyMovementMessage in pool");
                    return;
                }

                DummyMovementMessage dm = (DummyMovementMessage)m;

                dm.PlayerId = player.Id;
                dm.Position = pos;
                dm.Direction = dir;

                foreach(var other in PlayerInMap.Values)
                {
                    if (other.Id.Equals(player.Id)) continue;
                    other.SendMessageAsync(dm);
                }
            };
        }

        private void RegisterDisconnect(Player player)
        {
            void PlayerDisconnect(Player player)
            {
                player.OnDisconnect -= PlayerDisconnect;
                HandlePlayerDisconnect(player);
            }

            player.OnDisconnect += PlayerDisconnect;
        }

        protected override void OnPlayerRejoin(Player player)
        {
            GinLogger.Print($"Player[id:{player.Id} rejoined map[id:{Id}]");
            LetPlayerJoin(player) ;
        }

        private void HandlePlayerDisconnect(Player player)
        {
            PlayerLeave(player);
        }

        protected override void OnPlayerLeave(Player player)
        {
            GinLogger.Print($"Player[id:{player.Id} left map[id:{this.Id}");
            PlayerInMap.TryRemove(player.Id, out var removed);
            if (removed is not null)
            {
                NotifyLeaveToOthers(player);
            }
            
        }

        public void SendPlayersInMapToPlayer(Player requestedPlayer)
        {
            GinLogger.Debug("Sending dummies to joined player");
            foreach (var player in PlayerInMap.Values)
            {
                if (player.Id.Equals(requestedPlayer.Id)) continue;
                Message? m = MessagePool.Instance.Get(MessageTag.PlayerJoinMap);
                if (m is null or not PlayerJoinMapMessage)
                {
                    GinLogger.Debug("Can't find PlayerJoinMapMessage in pool");
                    return;
                }

                PlayerJoinMapMessage pjm = (PlayerJoinMapMessage)m;
                pjm.PlayerId = player.Id;
                pjm.DisplayName = player.DisplayName;
                pjm.Diamond = player.Diamond;
                pjm.Level = player.Level;
                pjm.Gold = player.Gold;
                pjm.Gender = player.Gender;
                pjm.Exp = player.Exp;
                pjm.LevelUpExp = player.LevelUpExp;
                pjm.Energy = player.Energy;
                pjm.EnergyCapacity = player.EnergyCapacity;
                pjm.MapId = this.Id;
                pjm.Position = new Vector3((float)player.PositionX, (float)player.PositionY, (float)player.PositionZ);
                pjm.Direction = new Vector3(0, 0, -1f);
                requestedPlayer.SendMessageAsync(pjm);
            }
        }

        private void NotifyJoinToOthers(Player joined)
        {
            Message? m = MessagePool.Instance.Get(MessageTag.PlayerJoinMap);
            if (m is null or not PlayerJoinMapMessage)
            {
                GinLogger.Debug("Can't find PlayerJoinMapMessage in pool");
                return;
            }

            PlayerJoinMapMessage pjm = (PlayerJoinMapMessage)m;
            pjm.PlayerId = joined.Id;
            pjm.DisplayName = joined.DisplayName;
            pjm.Diamond = joined.Diamond;
            pjm.Level = joined.Level;
            pjm.Gold = joined.Gold;
            pjm.Gender = joined.Gender;
            pjm.Exp = joined.Exp;
            pjm.LevelUpExp = joined.LevelUpExp;
            pjm.Energy = joined.Energy;
            pjm.EnergyCapacity = joined.EnergyCapacity;
            pjm.MapId = this.Id;
            pjm.Position = new Vector3((float)joined.PositionX, (float)joined.PositionY, (float)joined.PositionZ);
            pjm.Direction = new Vector3(0, 0, -1f);

            foreach (var player in PlayerInMap.Values)
            {
                if (player.Id.Equals(joined.Id)) continue;

                player.SendMessageAsync(pjm);
            }
        }

        private void NotifyLeaveToOthers(Player leftPlayer) 
        {
            foreach (var player in PlayerInMap.Values)
            {
                Message? m = MessagePool.Instance.Get(MessageTag.PlayerLeaveMap);
                if (m is null or not PlayerLeaveMapMessage)
                {
                    GinLogger.Debug("Can't find PlayerLeaveMapMessage in pool");
                    return;
                }

                PlayerLeaveMapMessage plm = (PlayerLeaveMapMessage)m;
                plm.PlayerId = leftPlayer.Id;
                plm.MapId = this.Id;
                player.SendMessageAsync(plm);
            }
        }
    }
}
    