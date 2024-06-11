using System;
using System.Collections.Generic;

namespace WindyFarm.Gin.Database.Models;

public partial class InventorySlot
{
    public Guid PlayerId { get; set; }

    public int Slot { get; set; }

    public Guid? ItemData { get; set; }

    public int StackCount { get; set; }

    public virtual ItemDat? ItemDataNavigation { get; set; }

    public virtual PlayerDat Player { get; set; } = null!;
}
