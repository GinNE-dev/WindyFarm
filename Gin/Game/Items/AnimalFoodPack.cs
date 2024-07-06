using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class AnimalFoodPack : AnimalFood
    {
        public override int Id => ItemId.ANIMAL_FOOD_PACK;

        public override string Name => "Animal food pack";

        public override string Description => "Use to feed cattles and poultries";
    }
}
