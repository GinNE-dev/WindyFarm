using System;
using System.Collections.Generic;

namespace WindyFarm.Gin.Data;

public partial class BarnDat
{
    public Guid OwnerId { get; set; }

    public int SlotIndex { get; set; }

    public int SpawnerId { get; set; }

    public int AnimalHealth { get; set; }

    public DateTime GrowAt { get; set; }

    public DateTime LastFeedAt { get; set; }

    public DateTime GiveProductAt { get; set; }

    public DateTime LastTimeMarkerUpdate { get; set; }

    public virtual PlayerDat Owner { get; set; } = null!;
}
