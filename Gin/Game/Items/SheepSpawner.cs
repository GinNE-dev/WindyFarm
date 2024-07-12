using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class SheepSpawner : CreatureSpawner
    {
        public override List<int> StageGrowingTimes => new() { 40 };

        public override int FeedTime => 3;

        public override int ProduceTime => 5;

        public override Item HarvestProduct => ItemReplicator.Get(ItemId.WOOL);

        public override int Id => ItemId.SHEEP_SPAWNER;

        public override string Name => "Sheep";

        public override string CreatureName => "Sheep";
    }
}
