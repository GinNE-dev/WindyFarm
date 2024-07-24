using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class Lettuce : Food, CraftingMaterial
    {
        public override int Energy => 10;

        public override int Id => ItemId.LETTUCE;

        public override string Name => "Lettuce";

        public Item CraftingProduct => ItemReplicator.Get(ItemId.LETTUCE_SEED);

        public int CraftingTime => 60;

        public int OutputRate => 2;
    }
}
