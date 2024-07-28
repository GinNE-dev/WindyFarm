using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Data;
using WindyFarm.Gin.Game.Crafting;
using WindyFarm.Gin.Game.Farming;
using WindyFarm.Gin.Game.Items;
using WindyFarm.Gin.Game.Mailing;
using WindyFarm.Gin.Game.Maps;
using WindyFarm.Gin.Network;
using WindyFarm.Gin.Network.Protocol;
using WindyFarm.Gin.Network.Protocol.Game;
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
        public int Energy => _playerData.Energy;
        public int Exp => _playerData.Exp;
        public string Gender => _playerData.Gender;
        public int MaxInventory => _playerData.MaxInventory;
        public string Email => _playerData.AccountId;
        public double PositionX => _playerData.PositionX;
        public double PositionY => _playerData.PositionY;
        public double PositionZ => _playerData.PositionZ;
        public int MapId => _playerData.MapId;
        public DateTime LastActiveAt => _playerData.LastActiveAt;
        public readonly Inventory Inventory;
        public readonly PlayerDat _playerData;
        private readonly Server _server;
        private readonly Session _session;
        private readonly WindyFarmDatabaseContext _dbContext;
        public readonly Farming.Farm FarmManager;
        public readonly Fabricator Fabricator;
        public readonly Barn Barn;
        public readonly MailBox MailBox;
        public event Action<Player> OnDisconnect = delegate { };
        public event Action<Vector3, Vector3> OnMove = delegate { };
        private readonly List<FriendshipDat> Friends = new();
        private readonly List<FriendshipDat> FriendInvitations = new();
        public Player(WindyFarmDatabaseContext dbContext, Server server, Session session, PlayerDat playerData)
        {
            _dbContext = dbContext;
            _playerData = playerData;
            _server = server;
            _session = session;
            _session.OnDisconnect += HandleDisconnect;
            Inventory = new Inventory(this, _dbContext);
            FarmManager = new Farming.Farm(this, _dbContext);
            Barn = new Barn(this, _dbContext);
            Fabricator = new Fabricator(this, _dbContext);
            MailBox = new MailBox(this, _dbContext);
            LoadFriend();
            JoinMap();
            OnlinePlayerManager.Instance.AddPlayer(this);
        }

        private void HandleDisconnect()
        {
            OnlinePlayerManager.Instance.RemovePlayer(this);
            _playerData.LastActiveAt = DateTime.Now;
            OnDisconnect(this);
        }

        public void JoinMap()
        {
            var map = MapManager.Instance.Get(MapId);
            if (map is not null)
            {
                map.PlayerJoin(this);
            }
        }
        public void ChangeMap(int  mapId)
        {
            _playerData.MapId = mapId;
        }

        private void LoadFriend()
        {
            var friendShips = _dbContext.FriendshipDats
                .Include(f=>f.Player)
                .Include(f=>f.OtherPlayer)
                .Where(fs=>fs.PlayerId.Equals(this.Id) || fs.OtherPlayerId.Equals(this.Id))
                .ToList();

            foreach (var friendship in friendShips)
            {
                var loweredStatus = friendship.FriendshipStatus.ToLower();
                if (loweredStatus.Equals("invite"))
                {
                    FriendInvitations.Add(friendship);
                }
                else if (loweredStatus.Equals("friend"))
                {
                    Friends.Add(friendship);
                }
            }
        }

        public void AcceptFriendInviteFromPlayer(Guid playerId)
        {
            GinLogger.Print($"AcceptFriendInviteFromPlayer: {_dbContext.FriendshipDats.Count()}");
            var rs = FriendInvitations.FirstOrDefault(r=>r.PlayerId.Equals(playerId));
            if (rs is null) return;
            rs.FriendshipStatus = "Friend";
            FriendInvitations.Remove(rs);
            GinLogger.Print($"AcceptFriendInviteFromPlayer: {_dbContext.FriendshipDats.Count()}");
            Friends.Add(rs);
            SendFriendInvitationList();
            GinLogger.Print($"AcceptFriendInviteFromPlayer: {_dbContext.FriendshipDats.Count()}");
            var onlinePlayer = OnlinePlayerManager.Instance.GetPlayer(playerId);
            if (onlinePlayer is null) return;
            GinLogger.Print($"AcceptFriendInviteFromPlayer: {_dbContext.FriendshipDats.Count()}");
            onlinePlayer.ReceiveFriendship(rs);
        }

        public void RejectFriendInviteFromPlayer(Guid playerId)
        {
            var rs = FriendInvitations.FirstOrDefault(r => r.PlayerId.Equals(playerId));
            if (rs is null) return;
            FriendInvitations.Remove(rs);
            _dbContext.FriendshipDats.Remove(rs);
            _dbContext.SaveChanges();

            SendFriendInvitationList();
        }

        public void InviteFriend(Guid playerId)
        {
            var existed = _dbContext.FriendshipDats.FirstOrDefault(f=>
            f.PlayerId.Equals(playerId) && f.OtherPlayerId.Equals(this.Id) ||
            f.PlayerId.Equals(this.Id) && f.OtherPlayerId.Equals(playerId));

            if (existed is not null) return;

            var otherPlayerDat = _dbContext.PlayerDats.FirstOrDefault(p=>p.Id.Equals(playerId));
            if(otherPlayerDat is null) return;
            var friendShip = new FriendshipDat();
            friendShip.PlayerId = this.Id;
            friendShip.OtherPlayerId = playerId;
            friendShip.Player = this._playerData;
            friendShip.OtherPlayer = otherPlayerDat;
            friendShip.FriendshipStatus = "Invite";
            friendShip.AchieveRelationshipAt = DateTime.Now;

            _dbContext.FriendshipDats.Add(friendShip);
            _dbContext.SaveChanges();
            GinLogger.Print($"InviteFriend: {_dbContext.FriendshipDats.Count()}");
            var onlinePlayer = OnlinePlayerManager.Instance.GetPlayer(playerId);
            if (onlinePlayer is null) return;
            onlinePlayer.ReceiveFriendInviation(friendShip);
        }

        public void Unfriend(Guid friendId)
        {
            GinLogger.Print($"Unfriend: {_dbContext.FriendshipDats.Count()}");
            var existedFriendship = _dbContext.FriendshipDats.FirstOrDefault(f =>
            f.PlayerId.Equals(friendId) && f.OtherPlayerId.Equals(this.Id) ||
            f.PlayerId.Equals(this.Id) && f.OtherPlayerId.Equals(friendId));
            GinLogger.Print("Unfriend");
            if(existedFriendship is null) return;
            GinLogger.Print("Unfriend b");
            _dbContext.FriendshipDats.Remove(existedFriendship);
            _dbContext.SaveChanges();
            Friends.Remove(existedFriendship);

            SendFriendList();

            var onlinePlayer = OnlinePlayerManager.Instance.GetPlayer(friendId);
            if (onlinePlayer is null) return;
            onlinePlayer.ReceiveUnfriend(existedFriendship);

            
        }

        public void ReceiveFriendInviation(FriendshipDat invitation)
        {
            if(!FriendInvitations.Contains(invitation)) FriendInvitations.Add(invitation);
        }

        public void ReceiveFriendship(FriendshipDat invitation)
        {
            if (!Friends.Contains(invitation)) Friends.Add(invitation);
        }

        public void ReceiveUnfriend(FriendshipDat friendship)
        {
            Friends.Remove(friendship);
        }

        public void SendFriendList()
        {
            Message? m = MessagePool.Instance.Get(MessageTag.FriendListResponse);
            if (m is null or not FriendListDataMessage) return;
            var flm = (FriendListDataMessage)m;

            flm.PlayerIds.Clear();
            flm.PlayerNames.Clear();
            flm.LastActiveTimes.Clear();

            foreach(var friendshipDat in Friends)
            {
                var playerDat = friendshipDat.PlayerId.Equals(this.Id) ? friendshipDat.OtherPlayer : friendshipDat.Player;
                flm.PlayerIds.Add(playerDat.Id);
                flm.PlayerNames.Add(playerDat.DisplayName);
                flm.LastActiveTimes.Add(OnlinePlayerManager.Instance.IsOnline(playerDat.Id) ? -1 : (int)(DateTime.Now - playerDat.LastActiveAt).TotalSeconds);
            }

            SendMessageAsync(flm);
        }

        public void SendFriendInvitationList()
        {
            Message? m = MessagePool.Instance.Get(MessageTag.FriendInviteListResponse);
            if (m is null or not FriendInviteListDataMessage) return;
            var flm = (FriendInviteListDataMessage)m;

            flm.PlayerIds.Clear();
            flm.PlayerNames.Clear();
            flm.InviteAts.Clear();

            foreach (var friendshipDat in FriendInvitations)
            {
                var playerDat = friendshipDat.Player;
                flm.PlayerIds.Add(playerDat.Id);
                flm.PlayerNames.Add(playerDat.DisplayName);
                flm.InviteAts.Add(friendshipDat.AchieveRelationshipAt);
            }

            SendMessageAsync(flm);
        }

        public bool SendStats()
        {
            Message? msg = MessagePool.Instance.Get(MessageTag.PlayerDataResponse);
            if (msg != null && msg is PlayerDataMessage)
            {
                PlayerDataMessage dataMsg = (PlayerDataMessage)msg;
                dataMsg.Id = this.Id;
                dataMsg.DisplayName = this.DisplayName;
                dataMsg.Gold = this.Gold;
                dataMsg.Diamond = this.Diamond;
                dataMsg.Level = this.Level;
                dataMsg.Exp = this.Exp;
                dataMsg.Energy = this.Energy;
                dataMsg.EnergyCapacity = this.EnergyCapacity;
                dataMsg.LevelUpExp = this.LevelUpExp;
                dataMsg.PositionX = this.PositionX;
                dataMsg.PositionY = this.PositionY;
                dataMsg.PositionZ = this.PositionZ;
                dataMsg.MaxInventory = this.MaxInventory;
                dataMsg.Gender = this.Gender;
                dataMsg.AccountId = this.Email;
                dataMsg.MapId = this.MapId;

                SendMessageAsync(dataMsg);
            }
            else
            {
                GinLogger.Debug($"{GetType().Name}: PlayerDataMessage not found!");
                return false;
            }
            
            return true;
        }

        public bool SendPosition()
        {
            Message? m = MessagePool.Instance.Get(MessageTag.PlayerMovement);
            if (m is null || m is not PlayerMovementMessage) return false;

            var movementMessage = (PlayerMovementMessage)m;
            /*
            movementMessage.PositionX = this.PositionX;
            movementMessage.PositionY = this.PositionY;
            movementMessage.PositionZ = this.PositionZ;
            */
            movementMessage.Position = new Vector3((float) this.PositionX, (float) this.PositionY, (float) this.PositionZ);

            SendMessageAsync(movementMessage);
            return true;
        }

        public bool SendInventory()
        {
            Message? msg = MessagePool.Instance.Get(MessageTag.InventoryDataResponse);
            if (msg == null || msg is not InventoryResponseMessage) return false;
            
            InventoryResponseMessage m = (InventoryResponseMessage)msg;
            var slotIndexes = Inventory.Slots.Keys.Order();
            m.ItemIds = new(slotIndexes.Count());
            m.StackCounts = new(slotIndexes.Count());
            m.MetaDataList = new(slotIndexes.Count());

            foreach (int slotIndex in slotIndexes)
            {
                var slot = Inventory.Slots[slotIndex];
                m.ItemIds.Add(slot.Item.Id);
                m.StackCounts.Add(slot.StackCount);
                if (slot.SlotData.ItemDat is not null) m.MetaDataList.Add(slot.SlotData.ItemDat);
            }

            return SendMessageAsync(m);
        }

        public void SendTopList(TopField topField)
        {
            TopListDataMessage messageData = new() { TopField = topField };

            IQueryable<PlayerDat> topList;
            switch (topField)
            {
                case TopField.Level:
                    topList = _dbContext.PlayerDats.OrderByDescending(p => p.Level).Take(10);
                    break;
                default:
                    topList = _dbContext.PlayerDats.OrderByDescending(p => p.Gold).Take(10);
                    break;
            }

            messageData.PlayerIdList = [.. topList.Select(p => p.Id)];
            messageData.DisplayNames = topList.Select(p => p.DisplayName).ToList();
            messageData.GoldList = [.. topList.Select(p => p.Gold)];
            messageData.Levels = [.. topList.Select(p => p.Level)];

            SendMessageAsync(messageData);
        }

        public int LevelUpExp => (int)Math.Round(86.95 * Math.Pow(1.15, Level));

        public int EnergyCapacity => 48 + Level * 2;

        private object energySafe = new();
        public bool TryConsumeEnergy(int amount)
        {
            if(amount < 0) return false;
            lock(energySafe)
            {
                if(Energy - amount < 0) return false;
                _playerData.Energy -= amount;
                return true;
            }
        }

        public void ConsumeFood(Food food)
        {
            if(food is null) return;
            GainEnergy(food.Energy);
        }

        public void GainEnergy(int amount)
        {
            lock (energySafe)
            {
                _playerData.Energy = Math.Min(Energy + amount, EnergyCapacity);
            }
        }

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

        public void SendMessage(IMessage message)
        {
            _session.SendMessage(message);
        }

        public void MoveTo(Vector3 position, Vector3 direction)
        {
            _playerData.PositionX = position.X;
            _playerData.PositionY = position.Y;
            _playerData.PositionZ = position.Z;
            OnMove(position, direction);
        }


        private object moneySafe = new();
        public bool GiveMoney(int amount)
        {
            lock (moneySafe)
            {
                _playerData.Gold += amount;
            }

            return true;
        }

        public bool TryTakeMoney(int amount)
        {
            lock (moneySafe)
            {
                if (_playerData.Gold < amount) return false;
                _playerData.Gold -= amount;
                return true;
            }
        }
    }
}
