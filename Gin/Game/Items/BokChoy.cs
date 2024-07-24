using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class BokChoy : Food, CraftingMaterial
    {
        public override int Energy => 10;

        public override int Id => ItemId.BOKCHOY;

        public override string Name => "BokChoy";

        public Item CraftingProduct => ItemReplicator.Get(ItemId.BOKCHOY_SEED);

        public int CraftingTime => 300;

        public int OutputRate => 2;
    }
}
