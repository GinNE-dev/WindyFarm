using System;
using System.Collections.Generic;

namespace WindyFarm.Gin.Data;

public partial class PlayerDat
{
    public Guid Id { get; set; }

    public string DisplayName { get; set; } = null!;

    public int Diamond { get; set; }

    public int Gold { get; set; }

    public int Level { get; set; }

    public int Energy { get; set; }

    public int Exp { get; set; }

    public string Gender { get; set; } = null!;

    public int MaxInventory { get; set; }

    public double PositionX { get; set; }

    public double PositionY { get; set; }

    public double PositionZ { get; set; }

    public int MapId { get; set; }

    public string AccountId { get; set; } = null!;

    public DateTime LastActiveAt { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<BarnDat> BarnDats { get; set; } = new List<BarnDat>();

    public virtual ICollection<FarmlandDat> FarmlandDats { get; set; } = new List<FarmlandDat>();

    public virtual ICollection<FriendshipDat> FriendshipDatOtherPlayers { get; set; } = new List<FriendshipDat>();

    public virtual ICollection<FriendshipDat> FriendshipDatPlayers { get; set; } = new List<FriendshipDat>();

    public virtual ICollection<InventorySlotDat> InventorySlotDats { get; set; } = new List<InventorySlotDat>();

    public virtual ICollection<MailDat> MailDatPlayerOnes { get; set; } = new List<MailDat>();

    public virtual ICollection<MailDat> MailDatPlayerTwos { get; set; } = new List<MailDat>();

    public virtual ICollection<MailMessage> MailMessageReceivers { get; set; } = new List<MailMessage>();

    public virtual ICollection<MailMessage> MailMessageSenders { get; set; } = new List<MailMessage>();
}
