using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class Potato : Food
    {
        public override int Energy => 10;

        public override int Id => ItemId.POTATO;

        public override string Name => "Potato";
    }
}
