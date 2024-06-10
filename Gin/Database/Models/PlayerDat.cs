using System;
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

    public virtual Account? Account { get; set; }
}
