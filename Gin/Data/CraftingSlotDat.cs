using System;
using System.Collections.Generic;

namespace WindyFarm.Gin.Data;

public partial class CraftingSlotDat
{
    public Guid OwnerId { get; set; }

    public int SlotIndex { get; set; }

    public int MaterialId { get; set; }

    public int MaterialQuality { get; set; }

    public int InputAmount { get; set; }

    public DateTime CompleteAt { get; set; }
}
