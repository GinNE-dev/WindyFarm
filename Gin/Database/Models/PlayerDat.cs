﻿using System;
using System.Collections.Generic;

namespace WindyFarm.Gin.Database.Models;

public partial class PlayerDat
{
    public Guid Id { get; set; }

    public string DisplayName { get; set; } = null!;

    public int Diamond { get; set; }

    public int Gold { get; set; }

    public int Level { get; set; }

    public int Exp { get; set; }

    public string Gender { get; set; } = null!;

    public int MaxInventory { get; set; }

    public double PositionX { get; set; }

    public double PositionY { get; set; }

    public double PositionZ { get; set; }

    public int MapId { get; set; }

    public string AccountId { get; set; } = null!;

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<InventorySlot> InventorySlots { get; set; } = new List<InventorySlot>();
}
