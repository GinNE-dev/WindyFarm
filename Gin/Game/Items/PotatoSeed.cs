using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class PotatoSeed : Seed
    {
        public override int Id => ItemId.POTATO_SEED;

        public override string Name => "Potato Seed";
        public sealed override List<int> StageGrowingTimes => new() {10, 10, 10};
        public sealed override Item HavestProduct => ItemReplicator.Get(ItemId.POTATO);
    }
}
