using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public interface CraftingMaterial : IItem
    {
        public Item CraftingProduct { get; }
        public int CraftingTime { get; }
        public int OutputRate {  get; } 
    }
}
