using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class Pumpkin : Food, CraftingMaterial
    {
        public override int Energy => 100;

        public override int Id => ItemId.PUMPKIN;

        public override string Name => "Pumpkin";

        public Item CraftingProduct => ItemReplicator.Get(ItemId.PUMPKIN_SEED);

        public int CraftingTime => 240;

        public int OutputRate => 2;
    }
}
