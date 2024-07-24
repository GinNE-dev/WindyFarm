using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindyFarm.Gin.Game.Items
{
    public interface IItem
    {
        public int Id { get; }
        public string Name { get; }
        public string Description { get; }
        public Guid DataId { get; }
        public int Quality { get; }
    }
}
