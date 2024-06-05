using System;
using System.Collections.Generic;

namespace WindyFarm.Gin.Database.Models;

public partial class Account
{
    public string Email { get; set; } = null!;

    public string HashedPassword { get; set; } = null!;

    public Guid? PlayerId { get; set; }

    public virtual Player? Player { get; set; }
}
