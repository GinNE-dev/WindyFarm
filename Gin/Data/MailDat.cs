using System;
using System.Collections.Generic;

namespace WindyFarm.Gin.Data;

public partial class MailDat
{
    public Guid MailId { get; set; }

    public Guid PlayerOneId { get; set; }

    public Guid PlayerTwoId { get; set; }

    public string PlayerOneTreat { get; set; } = null!;

    public string PlayerTwoTreat { get; set; } = null!;

    public DateTime UpdateAt { get; set; }

    public virtual ICollection<MailMessage> MailMessages { get; set; } = new List<MailMessage>();

    public virtual PlayerDat PlayerOne { get; set; } = null!;

    public virtual PlayerDat PlayerTwo { get; set; } = null!;
}
