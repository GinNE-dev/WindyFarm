using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Data;

namespace WindyFarm.Gin.Game.Items
{
    public abstract class Item : ICloneable
    {
        public abstract int Id { get; }
        public abstract string Name { get; }
        public abstract string Description { get; }
        public Guid DataId => itemData.Id;
        public int Quality => itemData.Quality;
        public ItemDat itemData { get; private set;}

        public Item()
        {
            itemData = new ItemDat();
        }

        public bool AssignMetaData(ItemDat itemMeta)
        {
            if (itemData == null) return false;
            itemData = itemMeta;
            return true;
        }

        public virtual string Detail() => Description;

        public object Clone() => MemberwiseClone();

        public bool HasSameData(Item item)
        {
            if(item is null) return false;
            return this.Quality.Equals(item.Quality);
        }

        public bool Equals(Item other)
        {
            if(other is null) return false;
            return this.Id.Equals(other.Id) && this.HasSameData(other);
        }
    }
}
