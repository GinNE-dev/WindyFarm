using System;
using System.Collections.Generic;

namespace WindyFarm.Gin.Database.Models;

public partial class ItemDat
{
    public Guid Id { get; set; }

    public int ItemType { get; set; }

    public int Quality { get; set; }

    public virtual ICollection<InventorySlot> InventorySlots { get; set; } = new List<InventorySlot>();
}
