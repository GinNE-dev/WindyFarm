using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Game.Items;
using WindyFarm.Gin.Game.Players;
using WindyFarm.Gin.Game.Shop;
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
            _player.SendPosition();
            _player.SendStats();
       
            return true;
        }

        public override bool handlePlayerMovement(PlayerMovementMessage message)
        {
            _player.MoveTo(message.PositionX, message.PositionY, message.PositionZ);
            return true;
        }

        public override bool handleInventoryRequest(InventoryRequestMessage message)
        {        
            return _player.SendInventory();
        }

        public override bool handleInventoryTransaction(InventoryTransactionMessage message)
        {
            _player.Inventory.HandleSlotTransction(message.OriginalSlotIndex, message.DestinationSlotIndex);
            _server.SaveDataAsync();
            return true;
        }

        public override bool handleFarmlandRequest(FarmlandRequestMessage message)
        {
            FarmlandResponseMessage m = new FarmlandResponseMessage();
            var farm = _player.FarmManager;
            var farmlandSlots = farm.GetOrderedPlots();
            foreach (var slot in farmlandSlots)
            {
                m.Seeds.Add(slot.Seed);
                m.CropQualities.Add(slot.CropQuality);
                m.FertilizeStats.Add(slot.Fertilized);
                m.PlotStates.Add(slot.PlotState);
                var item = ItemReplicator.Get(slot.Seed);
                if (item is Seed) 
                {
                    m.GrownTimes.Add(Math.Min((int) (DateTime.Now - slot.PlantedAt).TotalSeconds, ((Seed)item).StageGrowingTimes.Sum()));
                }
                else
                {
                    m.GrownTimes.Add(0);
                    
                }
            }

            _player.SendMessageAsync(m);
            return true;
        }

        public override bool handleFarmlandTransaction(FarmlandTransactionMessage message)
        {
            var farm = _player.FarmManager;
            switch(message.Action)
            {
                case FarmlandAction.Till:
                    farm.TillPlot(message.PlotIndex);
                    break;
                case FarmlandAction.Harvest:
                    farm.Harvest(message.PlotIndex);
                    break;
                case FarmlandAction.Fertilize:
                    farm.Fertilize(message.PlotIndex, message.UsedItemId, message.MetaDataId);
                    break;
                case FarmlandAction.Plant:
                    farm.SeedPlot(message.PlotIndex, message.UsedItemId, message.MetaDataId);
                    break;
                case FarmlandAction.Buy:
                    farm.BuyPlot(message.PlotIndex);
                    break;
                default:
                    return false;
            }

            return true;
        }

        public override bool handleFarmingShopRequest(FarmingShopRequestMessage message)
        {

            var shop = ShopManager.Instance.FarmingShop;
            
            if(shop is null)
            {
                GinLogger.Warning("FarmingShop is not innitialized!");
                return false;
            }

            var shopDataMessage = shop.PrepareShopDataMessage();
            if(shopDataMessage is null) return false;

            return _player.SendMessageAsync(shopDataMessage);
        }


        public override bool handleFarmingShopTransaction(FarmingShopTransactionMessage message)
        {
            var shop = ShopManager.Instance.FarmingShop;
            if (shop is null) return false;

            switch(message.Transaction)
            {
                case FarmingShopTransaction.Buy:
                    var buyItem = shop.GetItemAtSellingPlot(message.SlotIndex);
                    buyItem.AssignMetaData(new Data.ItemDat() { Id=Guid.NewGuid(), ItemType=buyItem.Id, Quality = 1});

                    var payAmount = shop.GetBuyPrice(buyItem.Id) * message.Quantity;
                    if (!_player.TryTakeMoney(payAmount)) return false;
                    GinLogger.Info($"Player[{_player.DisplayName}] paid {payAmount} coins to buy [{buyItem.Id}:{buyItem.Name}]x{message.Quantity}");
                    _player.SendStats();

                    if(!_player.Inventory.TryPutItem(buyItem, message.Quantity)) return false;
                    GinLogger.Info($"Player[{_player.DisplayName}] was received Item[{buyItem.Id}:{buyItem.Name}]x{message.Quantity}");

                    _player.SendInventory();
                    var shopDataMessage = shop.PrepareShopDataMessage();
                    if (shopDataMessage is null) return false;
                    _player.SendMessageAsync(shopDataMessage);
                    break;
                case FarmingShopTransaction.Sell:
                    var sellItem = _player.Inventory.TryTakeItemAt(message.SlotIndex, message.Quantity);
                    if(sellItem is null || sellItem is VoidItem) return false;
                    GinLogger.Info($"Player[{_player.DisplayName}] was taken Item[{sellItem.Id}:{sellItem.Name}]x{message.Quantity}");
                    _player.SendInventory();

                    var sellPrice = shop.GetSellPrice(sellItem.Id);
                    _player.GiveMoney(sellPrice * message.Quantity);
                    GinLogger.Info($"Player[{_player.DisplayName}] was given {sellPrice * message.Quantity} coins after sell [{sellItem.Id}:{sellItem.Name}]x{message.Quantity}");
                    _player.SendStats();

                    shopDataMessage = shop.PrepareShopDataMessage();
                    if (shopDataMessage is null) return false;
                    _player.SendMessageAsync(shopDataMessage);
                    break;
            }
            return true;
        }
    }
}
