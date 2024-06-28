using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class BokChoySeed : Seed
    {
        public override int Id => ItemId.BOKCHOY_SEED;

        public override string Name => "BokChoy Seed";
        public sealed override List<int> StageGrowingTimes => new() { 10, 10, 10, 10, 10 };
        public sealed override Item HarvestProduct => ItemReplicator.Get(ItemId.BOKCHOY);
    }
}
