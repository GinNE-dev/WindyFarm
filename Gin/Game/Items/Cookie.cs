using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class Cookie : Food
    {
        public override int Energy => 50;

        public override int Id => ItemId.COOKIE;

        public override string Name => "Cookie";
    }
}
