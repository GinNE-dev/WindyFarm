using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class EggPlantSeed : Seed
    {
        public override int Id => ItemId.EGGPLANT_SEED;

        public override string Name => "EggPlant Seed";
        public sealed override List<int> StageGrowingTimes => new() { 10, 10, 10, 10, 10 };
        public sealed override Item HarvestProduct => ItemReplicator.Get(ItemId.EGGPLANT);
    }
}
