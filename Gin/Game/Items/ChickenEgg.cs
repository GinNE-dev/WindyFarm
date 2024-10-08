﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class ChickenEgg : Food, CraftingMaterial
    {
        public override int Energy => 10;

        public override int Id => ItemId.CHICKEN_EGG;

        public override string Name => "Chicken Egg";

        public Item CraftingProduct => ItemReplicator.Get(ItemId.MAYONNAISE);

        public int CraftingTime => 300;

        public int OutputRate => 1;
    }
}
