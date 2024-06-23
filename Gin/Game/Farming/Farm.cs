using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Data;
using WindyFarm.Gin.Game.Players;

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
            for (int plotIndex = 0; plotIndex < FarmlandSize; plotIndex++)
            {
                var plotData = farmData.FirstOrDefault(p=>p.PlotIndex.Equals(plotIndex));
                if (plotData is null)
                {
                    plotData = new FarmlandDat()
                    {
                        OwnerId = _owner.Id,
                        PlotIndex = plotIndex,
                        PlotState = "Wild"
                    };
                    _dbContext.FarmlandDats.Add(plotData);
                    _dbContext.SaveChangesAsync();
                }

                Plots[plotIndex] = new FarmingPlot(plotData);
            }
        }

        public List<FarmingPlot> GetOrderedPlots() => Plots.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value).ToList();
    }
}
