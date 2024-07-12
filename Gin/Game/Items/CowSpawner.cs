using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class CowSpawner : CreatureSpawner
    {
        public override List<int> StageGrowingTimes => new() { 3 };

        public override int FeedTime => 1;

        public override int ProduceTime => 5;

        public override Item HarvestProduct => ItemReplicator.Get(ItemId.COW_MILK_JAR);

        public override int Id => ItemId.COW_SPAWNER;

        public override string Name => "Cow";

        public override string CreatureName => "Cow";
    }
}
