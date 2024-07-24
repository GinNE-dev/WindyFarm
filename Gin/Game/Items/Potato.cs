using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class Potato : Food, CraftingMaterial
    {
        public override int Energy => 10;

        public override int Id => ItemId.POTATO;

        public override string Name => "Potato";

        public Item CraftingProduct => ItemReplicator.Get(ItemId.POTATO_SEED);

        public int CraftingTime => 30;

        public int OutputRate => 2;
    }
}
