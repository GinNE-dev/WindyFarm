using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public class VoidItem : Item
    {
        public override int Id => ItemId.VOID_ITEM;

        public override string Name => "Empty";

        public override string Description => "Empty";
    }
}
