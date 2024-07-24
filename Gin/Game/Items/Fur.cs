using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class Fur : Item, CraftingMaterial
    {
        public override int Id => ItemId.FUR;

        public override string Name => "Fur";

        public override string Description => "Used to produce wool";

        public Item CraftingProduct => ItemReplicator.Get(ItemId.WOOL);

        public int CraftingTime => 500;

        public int OutputRate => 1;
    }
}
