using System;
using System.Collections.Generic;

namespace WindyFarm.Gin.Data;

public partial class FarmlandDat
{
    public Guid OwnerId { get; set; }

    public int PlotIndex { get; set; }

    public string PlotState { get; set; } = null!;

    public bool Fertilized { get; set; }

    public int CropQualityRiseChange { get; set; }

    public int Seed { get; set; }

    public int CropQuality { get; set; }

    public DateTime PlantedAt { get; set; }

    public virtual PlayerDat Owner { get; set; } = null!;
}
