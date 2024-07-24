using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class Milk : Food, CraftingMaterial
    {
        public override int Energy => 50;

        public override int Id => ItemId.COW_MILK_JAR;

        public override string Name => "Milk";

        public Item CraftingProduct => ItemReplicator.Get(ItemId.CHEESE);

        public int CraftingTime => 600;

        public int OutputRate => 1;
    }
}
