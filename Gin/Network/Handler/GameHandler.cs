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
                case FarmlandAction.Plant:
                    farm.SeedPlot(message.PlotIndex, message.Seed, message.SeedDataId);
                    break;
                case FarmlandAction.Buy:
                    farm.BuyPlot(message.PlotIndex);
                    break;
                default:
                    return false;
            }

            return true;
        }
    }
}
