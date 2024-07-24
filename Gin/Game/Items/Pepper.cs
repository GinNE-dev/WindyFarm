using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class Pepper : Food, CraftingMaterial
    {
        public override int Energy => 1;

        public override int Id => ItemId.PEPPER;

        public override string Name => "Pepper";

        public Item CraftingProduct => ItemReplicator.Get(ItemId.PEPPER_SEED);

        public int CraftingTime => 150;

        public int OutputRate => 2;
    }
}
