using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public abstract class Food : Item
    {
        public abstract int Energy { get; }
        public override string Description => $"Restore {Energy} energy when comsume";
    }
}
