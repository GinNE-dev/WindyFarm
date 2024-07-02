using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Data;
using WindyFarm.Gin.Game.Items;
using WindyFarm.Gin.SystemLog;

namespace WindyFarm.Gin.Game.Players
{
    public class InventorySlot
    {
        public int Index => SlotData.Slot;
        public Item Item { get; set; }
        public int StackCount => SlotData.StackCount;
        public readonly InventorySlotDat SlotData;
        public readonly Inventory _inventory;
        public InventorySlot(InventorySlotDat slotDat, Inventory inventory)
        {
            this._inventory = inventory;
            SlotData = slotDat;
            if (slotDat.ItemDat is not null)
            {
                Item = ItemReplicator.Get(slotDat.ItemDat.ItemType);
                Item.AssignMetaData(slotDat.ItemDat);
            }
            else
            {
                Item = new VoidItem();
            }
        }

        public void PutItem(Item item, int quantity)
        {
            if (quantity < 0) return;

            Item = item;
            SlotData.StackCount = quantity;
            SlotData.ItemDat = item.itemData;
            SlotData.ItemDatId = item.DataId;
        }

        
        public void RemoveItem()
        {
            var emptyData = _inventory.CreateEmptyData();
            Item = ItemReplicator.Get(ItemId.VOID_ITEM);
            Item.AssignMetaData(emptyData);

            SlotData.StackCount = 0;
            SlotData.ItemDat = Item.itemData;
            SlotData.ItemDatId = Item.DataId;
        }

        public bool IsEmpty()
        {
            return StackCount == 0 && (Item is null || Item is VoidItem);
        }

        private object slotSafe = new object();

        public T? TakeOne<T>() where T : Item
        {
            return Take<T>(1);
        }

        public T? Take<T>(int qty) where T : Item
        {
            if(qty < 0) return null;

            lock (slotSafe)
            {
                if (Item is not T) return null;

                if (SlotData.StackCount < qty)
                {
                    return null;
                }
                else if (SlotData.StackCount == qty)
                {
                    var item = Item;
                    RemoveItem();
                    return (T) item;
                }else
                {
                    SlotData.StackCount -= qty;
                    return (T) Item;
                }
            }
        }
    }
}
