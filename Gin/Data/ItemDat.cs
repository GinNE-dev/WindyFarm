using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WindyFarm.Gin.Data;

public partial class ItemDat
{
    public Guid Id { get; set; }

    public int ItemType { get; set; }

    public int Quality { get; set; }

    [JsonIgnore]
    public virtual ICollection<InventorySlotDat> InventorySlotDats { get; set; } = new List<InventorySlotDat>();
}
