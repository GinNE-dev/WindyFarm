using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class Turnip : Food, CraftingMaterial
    {
        public override int Energy => 11;

        public override int Id => ItemId.TURNIP;

        public override string Name => "Turnip";

        public Item CraftingProduct => ItemReplicator.Get(ItemId.TURNIP_SEED);

        public int CraftingTime => 90;

        public int OutputRate => 2;
    }
}
