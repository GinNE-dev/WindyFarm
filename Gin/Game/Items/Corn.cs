using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class Corn : Food, CraftingMaterial
    {
        public override int Energy => 10;

        public override int Id => ItemId.CORN;

        public override string Name => "Corn";

        public Item CraftingProduct => ItemReplicator.Get(ItemId.CORN_SEED);

        public int CraftingTime => 120;

        public int OutputRate => 2;
    }
}
