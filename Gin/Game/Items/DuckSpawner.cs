using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class DuckSpawner : CreatureSpawner
    {
        public override List<int> StageGrowingTimes => new() { 30 };

        public override int FeedTime => 3;

        public override int ProduceTime => 5;

        public override Item HarvestProduct => ItemReplicator.Get(ItemId.DUCK_EGG);

        public override int Id => ItemId.DUCK_SPAWNER;

        public override string Name => "Duck";

        public override string CreatureName => "Duck";
    }
}
