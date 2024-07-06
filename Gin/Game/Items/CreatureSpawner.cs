using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public abstract class CreatureSpawner : Item
    {
        public abstract string CreatureName { get; }
        public abstract List<int> StageGrowingTimes { get; }
        public abstract int FeedTime { get; }
        public abstract int ProduceTime { get; }
        public override string Description => "Lay on nest to receive creature";
        public abstract Item HarvestProduct { get; }
    }
}
