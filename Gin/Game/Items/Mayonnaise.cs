using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class Mayonnaise : Food
    {
        public override int Energy => 100;

        public override int Id => ItemId.MAYONNAISE;

        public override string Name => "Mayonnaise";
    }
}
