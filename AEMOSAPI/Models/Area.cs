using System;
using System.Collections.Generic;

namespace AEMOSAPI.Models;

public partial class Area
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public long? OrganizationId { get; set; }

    public long? ParentArea { get; set; }

    public string? Image { get; set; }

    public Guid? Uuid { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<AreaDevice> AreaDevices { get; set; } = new List<AreaDevice>();

    public virtual Organization? Organization { get; set; }
}
