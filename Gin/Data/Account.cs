using System;
using System.Collections.Generic;

namespace WindyFarm.Gin.Data;

public partial class Account
{
    public string Email { get; set; } = null!;

    public string HashedPassword { get; set; } = null!;

    public virtual PlayerDat? PlayerDat { get; set; }
}
