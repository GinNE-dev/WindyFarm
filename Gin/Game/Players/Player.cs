using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Data;
using WindyFarm.Gin.Game.Crafting;
using WindyFarm.Gin.Game.Farming;
using WindyFarm.Gin.Game.Items;
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
        public readonly Inventory Inventory;
        public readonly PlayerDat _playerData;
        private readonly Server _server;
        private readonly Session _session;
        private readonly WindyFarmDatabaseContext _dbContext;
        public readonly Farming.Farm FarmManager;
        public readonly Fabricator Fabricator;
        public readonly Barn Barn;
        public event Action<Player> OnDisconnect = delegate { };
        public event Action<Vector3, Vector3> OnMove = delegate { };
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

            JoinMap();
        }

        private void HandleDisconnect()
        {
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
