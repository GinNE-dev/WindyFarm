using System;
using System.Collections.Generic;

namespace WindyFarm.Gin.Data;

public partial class Inventory
{
    public Guid PlayerId { get; set; }

    public int Slot { get; set; }

    public int ItemId { get; set; }

    public int StackCount { get; set; }

    public virtual ItemDat Item { get; set; } = null!;

    public virtual PlayerDat Player { get; set; } = null!;
}
