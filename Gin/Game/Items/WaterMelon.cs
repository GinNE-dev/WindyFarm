using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class WaterMelon : Food, CraftingMaterial
    {
        public override int Energy => 30;

        public override int Id => ItemId.WATERMELON;

        public override string Name => "WaterMelon";

        public Item CraftingProduct => ItemReplicator.Get(ItemId.WATERMELON_SEED);

        public int CraftingTime => 240;

        public int OutputRate => 2;
    }
}
