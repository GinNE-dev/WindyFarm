using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class Carrot : Food, CraftingMaterial
    {
        public override int Energy => 10;

        public override int Id => ItemId.CARROT;

        public override string Name => "Carrot";

        public Item CraftingProduct => ItemReplicator.Get(ItemId.CARROT_SEED);

        public int CraftingTime => 120;

        public int OutputRate => 2;
    }
}
