using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class NormalFertilizer : Fertilizer
    {
        public override int QualityRiseChange => 50;


        public override int Id => ItemId.NORMAL_FERTILIZER;

        public override string Name => "Normal Fertilizer";

        public override string Description => $"Crop quality  rise {QualityRiseChange}%";
    }
}
