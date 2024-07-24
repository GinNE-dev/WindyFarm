using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindyFarm.Gin.Data;
using WindyFarm.Gin.Game.Items;
using WindyFarm.Gin.SystemLog;

namespace WindyFarm.Gin.Game.Crafting
{
    public class CraftingSlot
    {
        public int SlotIndex => _CraftingSlotDat.SlotIndex;
        public int MaterialId => _CraftingSlotDat.MaterialId;
        public int InputAmount => _CraftingSlotDat.InputAmount;
        public DateTime CompleteAt => _CraftingSlotDat.CompleteAt;
        public int MaterialQuality => _CraftingSlotDat.MaterialQuality;

        private readonly CraftingSlotDat _CraftingSlotDat;
        public CraftingSlot(CraftingSlotDat craftingSlotDat) 
        {
            _CraftingSlotDat = craftingSlotDat;
        }

        public void StartCraft(CraftingMaterial material, int inputAmount)
        {
            _CraftingSlotDat.MaterialId = material.Id;
            _CraftingSlotDat.MaterialQuality = material.Quality;
            _CraftingSlotDat.CompleteAt = DateTime.Now.AddSeconds(material.CraftingTime * inputAmount);
            _CraftingSlotDat.InputAmount = inputAmount;
        }

        public CraftingMaterial? GetCraftingMaterial()
        {
            var item = ItemReplicator.Get(_CraftingSlotDat.MaterialId);

            if (item is not CraftingMaterial) return null;

            return (CraftingMaterial) item;
        }
        
        public int GetOutputAmount()
        {
            var material = GetCraftingMaterial();
            if (material is null) return 0;

            return InputAmount * material.OutputRate;
        }

        public void GetProduct(out Item? product, out int quantity)
        {
            quantity = 0;
            product = null;
            if (_CraftingSlotDat.CompleteAt > DateTime.Now) return;
            var item = ItemReplicator.Get(_CraftingSlotDat.MaterialId);

            var material = GetCraftingMaterial();
            if (material is null) return;
            
            var p = material.CraftingProduct;
            var prductData = new ItemDat() { Id = Guid.NewGuid(), ItemType = p.Id, Quality = _CraftingSlotDat.MaterialQuality };
            p.AssignMetaData(prductData);

            quantity = _CraftingSlotDat.InputAmount * material.OutputRate;
            product = p;
        }

        public void CompleteCraft()
        {
            if (_CraftingSlotDat.CompleteAt > DateTime.Now) return;

            _CraftingSlotDat.MaterialId = 0;
            _CraftingSlotDat.MaterialQuality = 1;
            _CraftingSlotDat.InputAmount = 0;
        }

        public bool IsEmpty() => _CraftingSlotDat.MaterialId == 0;
    }
}
