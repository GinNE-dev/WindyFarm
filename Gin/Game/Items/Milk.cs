using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class Milk : Food
    {
        public override int Energy => 50;

        public override int Id => ItemId.COW_MILK_JAR;

        public override string Name => "Milk";
    }
}
