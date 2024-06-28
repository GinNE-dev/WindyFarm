using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class WaterMelonSeed : Seed
    {
        public override int Id => ItemId.WATERMELON_SEED;

        public override string Name => "WaterMelon Seed";
        public sealed override List<int> StageGrowingTimes => new() { 10, 10, 10, 10, 10 };
        public sealed override Item HarvestProduct => ItemReplicator.Get(ItemId.WATERMELON);
    }
}
