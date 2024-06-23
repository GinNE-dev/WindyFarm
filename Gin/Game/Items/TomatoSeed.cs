using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace WindyFarm.Gin.Game.Items
{
    public class TomatoSeed : Seed
    {
        public sealed override int Id => ItemId.TOMATO_SEED;

        public sealed override string Name => "Tomato Seed";

        public sealed override string Description => "Plant to grow tomato";

        public sealed override List<int> StageGrowingTimes => new() { 5, 5, 5 };

        public sealed override Item HavestProduct => ItemReplicator.Get(ItemId.TOMATO);
    }
}
