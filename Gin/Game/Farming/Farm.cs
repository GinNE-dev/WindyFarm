using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Data;
using WindyFarm.Gin.Game.Players;
using WindyFarm.Gin.Network.Protocol.Game;
using WindyFarm.Gin.Network.Protocol;
using WindyFarm.Gin.SystemLog;
using WindyFarm.Gin.Game.Items;

namespace WindyFarm.Gin.Game.Farming
{
    public class Farm
    {
        public readonly int FarmlandSize = 128;
        private readonly Player _owner;
        private readonly WindyFarmDatabaseContext _dbContext;
        private readonly ConcurrentDictionary<int, FarmingPlot> Plots = new();
        public Farm(Player owner, WindyFarmDatabaseContext dbContext) 
        {
            _dbContext = dbContext;
            _owner = owner;
            LoadFarmland();
        }

        private void LoadFarmland()
        {
            var farmData = _dbContext.FarmlandDats.Where(d=>d.OwnerId.Equals(_owner.Id)).ToList();
            bool isFirstInit = false;
            for (int plotIndex = 0; plotIndex < 10; plotIndex++)
            {
                var plotData1 = farmData.FirstOrDefault(p => p.PlotIndex.Equals(plotIndex));
                if (plotData1 is null)
                {
                    isFirstInit = true;
                    plotData1 = new FarmlandDat()
                    {
                        OwnerId = _owner.Id,
                        PlotIndex = plotIndex,
                        PlotState = "Messed"
                    };
                    _dbContext.FarmlandDats.Add(plotData1);
                }

                Plots[plotIndex] = new FarmingPlot(plotData1);
            }

            var plotData = farmData.FirstOrDefault(p => p.PlotIndex.Equals(10));
            if (plotData is null)
            {
                isFirstInit = true;
                plotData = new FarmlandDat()
                {
                    OwnerId = _owner.Id,
                    PlotIndex = 10,
                    PlotState = "Buyable"
                };
                _dbContext.FarmlandDats.Add(plotData);
                _dbContext.SaveChanges();
            }

            Plots[10] = new FarmingPlot(plotData);

            for (int plotIndex = 11; plotIndex < FarmlandSize; plotIndex++)
            {
                var plotData2 = farmData.FirstOrDefault(p=>p.PlotIndex.Equals(plotIndex));
                if (plotData2 is null)
                {
                    isFirstInit = true;
                    plotData2 = new FarmlandDat()
                    {
                        OwnerId = _owner.Id,
                        PlotIndex = plotIndex,
                        PlotState = "Wild"
                    };
                    _dbContext.FarmlandDats.Add(plotData2);
                }

                Plots[plotIndex] = new FarmingPlot(plotData2);
            }

            if(isFirstInit) _dbContext.SaveChanges();
        }

        public List<FarmingPlot> GetOrderedPlots() => Plots.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value).ToList();

        public void BuyPlot(int plotIdx)
        {
            if (plotIdx < 0 || plotIdx > FarmlandSize - 1) return;
            
            Plots.TryGetValue(plotIdx, out var farmingPlot);

            if (farmingPlot is null) return;
            int requiredMoney = (int)Math.Ceiling(458 * Math.Pow(1.08118, plotIdx));
            if (_owner.TryTakeMoney(requiredMoney))
            {
                farmingPlot.Buy();
                Plots.TryGetValue(plotIdx+1, out var nextPlot);
                if (nextPlot is null) return;
                nextPlot.AllowBuy();

                ResponseFarmlandTransaction(plotIdx, FarmlandAction.Buy, true);
                _owner.SendStats();
            }
        }

        public void TillPlot(int plotIdx)
        {
            if (plotIdx < 0 || plotIdx > FarmlandSize - 1) return;
            Plots.TryGetValue(plotIdx, out var farmingPlot);

            if (farmingPlot is null) return;
            if(!_owner.TryConsumeEnergy(FarmBalancer.TillPlotEnergyConsumtion))
            {
                ResponseFarmlandTransaction(plotIdx, FarmlandAction.Plant, false);
                return;
            }
            _owner.SendStats();

            farmingPlot.Till();

            ResponseFarmlandTransaction(plotIdx, FarmlandAction.Till, true);
        }

        public void SeedPlot(int plotIdx, int itemSeedId, Guid seedDataId)
        {
            if (plotIdx < 0 || plotIdx > FarmlandSize - 1) return;
            Plots.TryGetValue(plotIdx, out var farmingPlot);

            if (farmingPlot is null) return;
            if (!farmingPlot.IsAllowPlant()) return;
            if (!_owner.TryConsumeEnergy(FarmBalancer.SeedPlantEnergyConsumtion))
            {
                ResponseFarmlandTransaction(plotIdx, FarmlandAction.Plant, false, itemSeedId);
                return;
            }
            _owner.SendStats();

            var seed = _owner.Inventory.TryTakeOne<Seed>(itemSeedId, seedDataId);
            if (seed is null)
            {
                GinLogger.Warning($"Someone tried to seed with Item[id={itemSeedId}] that is not Seed!!!!");
                return;
            }

            farmingPlot.Plant(seed);
            _owner.SendInventory();

            ResponseFarmlandTransaction(plotIdx, FarmlandAction.Plant, true, itemSeedId);
        }

        public void Fertilize(int plotIdx, int fertilizerId, Guid dataId)
        {
            if (plotIdx < 0 || plotIdx > FarmlandSize - 1) return;
            Plots.TryGetValue(plotIdx, out var farmingPlot);

            if (farmingPlot is null) return;
            var fertilize = _owner.Inventory.TryTakeOne<Fertilizer>(fertilizerId, dataId);
            _owner.SendInventory();

            if (fertilize is null)
            {
                GinLogger.Warning($"Someone tried to fertilize with Item[id={fertilizerId}] item that is not Fertilizer!!!!");
                return;
            }

            farmingPlot.Fertilize(fertilize);

            ResponseFarmlandTransaction(plotIdx, FarmlandAction.Fertilize, true);
        }

        public void Harvest(int plotIdx)
        {
            if (plotIdx < 0 || plotIdx > FarmlandSize - 1) return;
            Plots.TryGetValue(plotIdx, out var farmingPlot);

            if (farmingPlot is null) return;

            if (!_owner.TryConsumeEnergy(FarmBalancer.HarvestEnergyConsumtion))
            {
                ResponseFarmlandTransaction(plotIdx, FarmlandAction.Harvest, false);
                return;
            }

            _owner.SendStats();

            var product = farmingPlot.GetHarvestProduct();
            if (product is VoidItem) return;

            if (_owner.Inventory.TryPutOne(product))
            {
                farmingPlot.ClearPlot();
                _owner.SendInventory();
            }

            ResponseFarmlandTransaction(plotIdx, FarmlandAction.Harvest, true);
        }

        private bool ResponseFarmlandTransaction(int plotIdx, FarmlandAction action, bool success, int plantedSeed = 0)
        {
            Message? m = MessagePool.Instance.Get(MessageTag.FarmlandTransactionResult);
            if (m is null) return false;
            var result = (FarmlandTransactionResultMessage)m;
            result.PlotIndex = plotIdx;
            result.Action = action;
            result.Success = success;
            result.PlantedSeed = plantedSeed;

            return _owner.SendMessageAsync(result);
        }
    }
}
