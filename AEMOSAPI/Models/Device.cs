using System;
using System.Collections.Generic;

namespace AEMOSAPI.Models;

public partial class Device
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool? Status { get; set; }

    public Guid? Uuid { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? State { get; set; }

    public string? Type { set; get; }

    public virtual ICollection<AreaDevice> AreaDevices { get; set; } = new List<AreaDevice>();

    public virtual ICollection<TelemetryDatum> TelemetryData { get; set; } = new List<TelemetryDatum>();
}
