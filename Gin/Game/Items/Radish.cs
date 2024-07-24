using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class Radish : Food, CraftingMaterial
    {
        public override int Energy => 20;

        public override int Id => ItemId.RADISH;

        public override string Name => "Radish";

        public Item CraftingProduct => ItemReplicator.Get(ItemId.RADISH_SEED);

        public int CraftingTime => 90;

        public int OutputRate => 2;
    }
}
