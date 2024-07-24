using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class Truffle : Food, CraftingMaterial
    {
        public override int Energy => 70;

        public override int Id => ItemId.TRUFFLE;

        public override string Name => "Truffle";

        public Item CraftingProduct => ItemReplicator.Get(ItemId.TRUFFLE_OIL);

        public int CraftingTime => 1200;

        public int OutputRate => 1;
    }
}
