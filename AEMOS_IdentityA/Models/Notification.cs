using System;
using System.Collections.Generic;

namespace AEMOS_IdentityA.Models;

public partial class Notification
{
    public long Id { get; set; }

    public string? Title { get; set; }

    public string? Insight { get; set; }

    public string? UserGroup { get; set; }

    public string? Severity { get; set; }

    public long? OrganizationId { get; set; }

    public virtual Organization? Organization { get; set; }
}
