using System;
using System.Collections.Generic;

namespace WindyFarm.Gin.Data;

public partial class InventorySlotDat
{
    public Guid PlayerId { get; set; }

    public int Slot { get; set; }

    public Guid? ItemDatId { get; set; }

    public int StackCount { get; set; }

    public virtual ItemDat? ItemDat { get; set; }

    public virtual PlayerDat Player { get; set; } = null!;
}
