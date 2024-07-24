using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    internal class Tomato : Food, CraftingMaterial
    {
        public sealed override int Id => ItemId.TOMATO;

        public sealed override string Name => "Tomato";
        public sealed override string Description => $"Restore {Energy} Energy when consume";
        public sealed override int Energy => 10;

        public int CraftingTime => 20;
        public Item CraftingProduct => ItemReplicator.Get(ItemId.TOMATO_SEED);

        public int OutputRate => 2;
    }
}
