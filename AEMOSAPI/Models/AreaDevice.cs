using System;
using System.Collections.Generic;

namespace AEMOSAPI.Models;

public partial class AreaDevice
{
    public long AreaId { get; set; }

    public long DeviceId { get; set; }

    public string? Detail { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Area Area { get; set; } = null!;

    public virtual Device Device { get; set; } = null!;
}
