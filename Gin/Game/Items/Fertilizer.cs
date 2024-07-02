using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public abstract class Fertilizer : Item
    {
        public abstract int QualityRiseChange { get; }
    }
}
