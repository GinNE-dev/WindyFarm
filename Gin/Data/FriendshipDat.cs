using System;
using System.Collections.Generic;

namespace WindyFarm.Gin.Data;

public partial class FriendshipDat
{
    public Guid PlayerId { get; set; }

    public Guid OtherPlayerId { get; set; }

    public string FriendshipStatus { get; set; } = null!;

    public DateTime AchieveRelationshipAt { get; set; }

    public virtual PlayerDat OtherPlayer { get; set; } = null!;

    public virtual PlayerDat Player { get; set; } = null!;
}
