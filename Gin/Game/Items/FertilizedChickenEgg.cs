using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class FertilizedChickenEgg : CreatureSpawner
    {
        public override List<int> StageGrowingTimes => new() {10};

        public override int FeedTime => 3;

        public override int ProduceTime => 5;

        public override Item HarvestProduct => ItemReplicator.Get(ItemId.CHICKEN_EGG);

        public override int Id => ItemId.FERTILIZED_CHICKEN_EGG;

        public override string Name => "Chicken";

        public override string CreatureName => "Chicken";
    }
}
