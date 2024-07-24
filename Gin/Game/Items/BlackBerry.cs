using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class BlackBerry : Food, CraftingMaterial
    {
        public override int Energy => 10;

        public override int Id => ItemId.BLACKBERRY;

        public override string Name => "BlackBerry";

        public Item CraftingProduct => ItemReplicator.Get(ItemId.BLACKBERRY_SEED);

        public int CraftingTime => 200;

        public int OutputRate => 2;
    }
}
