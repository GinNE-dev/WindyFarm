using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class EggPlant : Food, CraftingMaterial
    {
        public override int Energy => 10;

        public override int Id => ItemId.EGGPLANT;

        public override string Name => "Eggplant";

        public Item CraftingProduct => ItemReplicator.Get(ItemId.EGGPLANT_SEED);

        public int CraftingTime => 60;

        public int OutputRate => 2;
    }
}
