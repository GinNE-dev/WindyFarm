using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class StrawBerry : Food, CraftingMaterial
    {
        public override int Energy => 12;

        public override int Id => ItemId.STRAWBERRY;

        public override string Name => "StrawBerry";

        public Item CraftingProduct => ItemReplicator.Get(ItemId.STRAWBERRY_SEED);

        public int CraftingTime => 120;

        public int OutputRate => 2;
    }
}
