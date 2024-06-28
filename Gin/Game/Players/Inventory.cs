using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Data;
using WindyFarm.Gin.Game.Items;
using WindyFarm.Gin.SystemLog;

namespace WindyFarm.Gin.Game.Players
{
    public class Inventory
    {
        private readonly int MAX_STACK_COUNT = 999;
        public readonly ConcurrentDictionary<int, InventorySlot> Slots = new();
        private readonly WindyFarmDatabaseContext _dbContext;
        private readonly Player _owner;
        public Inventory(Player owner, WindyFarmDatabaseContext dbContext) 
        {
            _owner = owner;
            _dbContext = dbContext;


            InitInventory();
        }

        private void InitInventory()
        {
            var inventoryData = _dbContext.InventorySlotDats
                .Include(sld => sld.ItemDat)
                .Where(s => s.PlayerId.Equals(_owner.Id)).ToList();

            for (int slotIndex = 0; slotIndex < _owner.MaxInventory; slotIndex++)
            {
                var slotData = inventoryData.FirstOrDefault(d => d.Slot.Equals(slotIndex));
                if (slotData is null)
                {
                    var itemData = CreateEmptyData();
                    slotData = new()
                    {
                        PlayerId = _owner.Id,
                        Slot = slotIndex,
                        Player = _owner._playerData,
                        ItemDatId = itemData.Id,
                        ItemDat = itemData
                    };

                    _dbContext.InventorySlotDats.Add(slotData);
                    inventoryData.Add(slotData);
                }
            }

            foreach (var slotDat in inventoryData)
            {
                var slot = new InventorySlot(slotDat, this);
                Slots.TryAdd(slot.Index, slot);
            }
        }

        public ItemDat CreateEmptyData()
        {
            var emptyData = _dbContext.ItemDats.FirstOrDefault(x => x.Id.Equals(Guid.Empty) && x.ItemType.Equals(ItemId.VOID_ITEM));
            if(emptyData is null)
            {
                emptyData = new ItemDat() { Id = Guid.Empty, ItemType = ItemId.VOID_ITEM };
                _dbContext.ItemDats.Add(emptyData);
                _dbContext.SaveChanges();
            }
            
            return emptyData;
        }

        public void HandleSlotTransction(int orgIndex, int desIndex)
        {
            Slots.TryGetValue(orgIndex, out var orgSlot);
            Slots.TryGetValue(desIndex, out var desSlot);

            if (orgSlot is not null && desSlot is not null)
            {
                var desItemDat = desSlot.SlotData.ItemDat;

                var orgItemDat = orgSlot.SlotData.ItemDat;

                if(//TODO: Group
                    orgItemDat is not null 
                    && desItemDat is not null
                    && orgItemDat.Id.Equals(desItemDat.Id)
                    && orgItemDat.ItemType.Equals(desItemDat.ItemType))
                {
                    int groupQty = orgSlot.StackCount + desSlot.StackCount;
                    if (groupQty <= MAX_STACK_COUNT)
                    {
                        desSlot.PutItem(desSlot.Item, groupQty);
                        orgSlot.RemoveItem();
                    }
                    else
                    {
                        desSlot.PutItem(desSlot.Item, MAX_STACK_COUNT);
                        orgSlot.PutItem(orgSlot.Item, groupQty-MAX_STACK_COUNT);
                    }

                    return;
                }

                //Move
                if(desSlot.IsEmpty())
                {
                    desSlot.PutItem(orgSlot.Item, orgSlot.SlotData.StackCount);
                    orgSlot.RemoveItem();
                    return;
                }else
                {//, Swap
                    var desItem = desSlot.Item;
                    var desStackCount = desSlot.StackCount;

                    desSlot.PutItem(orgSlot.Item, orgSlot.SlotData.StackCount);
                    orgSlot.PutItem(desItem, desStackCount);
                }   
            }
        }

        public Item? FindItemUnique(int itemId, Guid dataId) 
            => Slots.FirstOrDefault(s=>s.Value.Item.Id.Equals(itemId) && s.Value.Item.DataId.Equals(dataId)).Value.Item;

        private object slotSafe = new();
        public Item? TryTakeItem(int itemId, Guid dataId, int qty)
        {
            lock (slotSafe)
            {
                var slot = Slots.FirstOrDefault(
                    s => s.Value.Item.Id.Equals(itemId) 
                    && s.Value.Item.DataId.Equals(dataId))
                    .Value;

                if (slot is null) return null;
                return slot.Take(qty);
            }
        }

        public Item? TryTakeOne(int itemId, Guid dataId)
        {
            return TryTakeItem(itemId, dataId, 1);
        }

        public bool IsAvailableFor(Item item, int qty)
        {
            lock (slotSafe)
            {
                var sameItemAvailable = Slots.Values
                    .Where(s => s.Item.Equals(item))
                    .OrderBy(s => s.StackCount).Sum(s=> MAX_STACK_COUNT-s.StackCount);

                var emptyAvailable = FindEmtySlot() * MAX_STACK_COUNT;

                return qty - (sameItemAvailable+emptyAvailable) <= 0;
            }
        }

        public InventorySlot? FirstEmptySlot() => Slots.Values.FirstOrDefault(s => s.IsEmpty());
        public int FindEmtySlot() => Slots.Values.Count(s=>s.IsEmpty());

        public bool TryPutItem(Item item, int qty)
        {
            lock (slotSafe)
            {
                if(!IsAvailableFor(item, qty)) return false;

                var sameItemSlots = Slots.Values
                    .Where(s => s.Item.Equals(item)).ToList();

                int remaining = qty;
                foreach (var slot in sameItemSlots)
                {
                    if (remaining <= 0) return true;
                    item.AssignMetaData(slot.Item.itemData);

                    var putAvl = Math.Min(MAX_STACK_COUNT-slot.StackCount, remaining);
                    slot.PutItem(slot.Item, slot.StackCount + putAvl);
                    remaining -= putAvl;
                }

                if (remaining > 0)
                {
                    var emtySlots = Slots.Values.Where(s => s.Item is VoidItem).ToList();
                    foreach (var slot in emtySlots)
                    {
                        if (remaining <= 0) return true;
                        var putAvl = Math.Min(MAX_STACK_COUNT, qty);
                        slot.PutItem(item, putAvl);
                        remaining -= putAvl;
                    }
                }               
                
                return true;    
            }
        }

        public bool TryPutOne(Item item)
        {
            return TryPutItem(item, 1);
        }
    }
}
