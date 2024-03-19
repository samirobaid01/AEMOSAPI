using System;
using System.Collections.Generic;

namespace AEMOSAPI.Models;

public partial class Organization
{
    public long Id { get; set; }

    public long? ParentId { get; set; }

    public string? Name { get; set; }

    public bool? Status { get; set; }

    public string? Detail { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? PaymentMethods { get; set; }

    public string? Image { get; set; }

    public string? Address { get; set; }

    public string? Zip { get; set; }

    public string? Email { get; set; }

    public bool? IsParent { get; set; }

    public string? ContactNumber { get; set; }

    public virtual ICollection<Area> Areas { get; set; } = new List<Area>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
