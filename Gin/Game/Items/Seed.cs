using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public abstract class Seed : Item
    {
        public abstract List<int> StageGrowingTimes { get; }
        public override string Description => "Plant to grow plant";
        public abstract Item HarvestProduct { get; }
    }
}
