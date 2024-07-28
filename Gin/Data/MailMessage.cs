using System;
using System.Collections.Generic;

namespace WindyFarm.Gin.Data;

public partial class MailMessage
{
    public Guid Id { get; set; }

    public Guid MailId { get; set; }

    public Guid SenderId { get; set; }

    public Guid ReceiverId { get; set; }

    public string MessageContent { get; set; } = null!;

    public DateTime SentAt { get; set; }

    public virtual MailDat Mail { get; set; } = null!;

    public virtual PlayerDat Receiver { get; set; } = null!;

    public virtual PlayerDat Sender { get; set; } = null!;
}
