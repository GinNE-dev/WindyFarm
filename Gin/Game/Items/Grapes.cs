using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class Grapes : Food, CraftingMaterial
    {
        public override int Energy => 10;

        public override int Id => ItemId.GRAPES;

        public override string Name => "Grapes";

        public Item CraftingProduct => ItemReplicator.Get(ItemId.GRAPES_SEED);

        public int CraftingTime => 120;

        public int OutputRate => 2;
    }
}
