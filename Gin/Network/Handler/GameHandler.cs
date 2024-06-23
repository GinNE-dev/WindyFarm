using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Game.Items;
using WindyFarm.Gin.Game.Players;
using WindyFarm.Gin.Network.Protocol;
using WindyFarm.Gin.Network.Protocol.Game;
using WindyFarm.Gin.SystemLog;

namespace WindyFarm.Gin.Network.Handler
{
    public class GameHandler : MessageHandler
    {
        private readonly Server _server;
        private readonly Session _session;
        private readonly Player _player;

        public GameHandler(Server server, Session session, IPlayer player) 
        {
            _server = server;
            _session = session;
            _player = (Player) player;
        }

        public override bool handlePing(PingMessage message)
        {
            Message? msg = MessagePool.Instance.Get(MessageTag.PingReply);
            if (msg != null && msg is PingReplyMessage)
            {
                _session.SendMessageAsync((PingReplyMessage) msg);
            }

            return true;
        }

        public override bool handlePlayerDataRequest(RequestPlayerMessage message)
        {
            Message? msg = MessagePool.Instance.Get(MessageTag.PlayerDataResponse);
            if(msg != null && msg is PlayerDataMessage)
            {
                PlayerDataMessage dataMsg = (PlayerDataMessage) msg;
                dataMsg.Id = _player.Id;
                dataMsg.DisplayName = _player.DisplayName;
                dataMsg.Gold = _player.Gold;
                dataMsg.Diamond = _player.Diamond;
                dataMsg.Level = _player.Level;
                dataMsg.Exp = _player.Exp;
                dataMsg.LevelUpExp = _player.LevelUpExp;
                dataMsg.PositionX = _player.PositionX;
                dataMsg.PositionY = _player.PositionY;
                dataMsg.PositionZ = _player.PositionZ;
                dataMsg.MaxInventory = _player.MaxInventory;
                dataMsg.Gender = _player.Gender;
                dataMsg.AccountId = _player.Email;
                dataMsg.MapId = _player.MapId;

                _session.SendMessageAsync(dataMsg);
            }else
            {
                GinLogger.Debug($"{GetType().Name}: PlayerDataMessage not found!");
            }
            
            return true;
        }

        public override bool handlePlayerMovement(PlayerMovementMessage message)
        {
            _player.MoveTo(message.PositionX, message.PositionY, message.PositionZ);
            return true;
        }

        public override bool handleInventoryRequest(InventoryRequestMessage message)
        {
            var inv = _player.Inventory;

            Message? msg = MessagePool.Instance.Get(MessageTag.InventoryDataResponse);
            if (msg != null && msg is InventoryResponseMessage)
            {
                InventoryResponseMessage m = (InventoryResponseMessage)msg;
                var slotIndexes = inv.Slots.Keys.Order();
                m.ItemIds = new(slotIndexes.Count());
                m.StackCounts = new(slotIndexes.Count());
                m.MetaDataList = new(slotIndexes.Count());

                foreach (int slotIndex in slotIndexes)
                {
                    var slot = inv.Slots[slotIndex];
                    m.ItemIds.Add(slot.Item.Id);
                    m.StackCounts.Add(slot.StackCount);
                    if(slot.SlotData.ItemDat is not null) m.MetaDataList.Add(slot.SlotData.ItemDat);
                }

                _player.SendMessageAsync(m);
            }
            
            return true;
        }

        public override bool handleInventoryTransaction(InventoryTransactionMessage message)
        {
            _player.Inventory.HandleSlotTransction(message.OriginalSlotIndex, message.DestinattionSlotIndex);
            _server.SaveDataAsync();
            return true;
        }
    }
}
