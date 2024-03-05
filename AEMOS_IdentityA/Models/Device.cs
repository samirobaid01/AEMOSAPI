using System;
using System.Collections.Generic;

namespace AEMOS_IdentityA.Models;

public partial class Device
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool? Status { get; set; }

    public Guid? Uuid { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<AreaDevice> AreaDevices { get; set; } = new List<AreaDevice>();

    public virtual ICollection<State> States { get; set; } = new List<State>();

    public virtual ICollection<TelemetryDatum> TelemetryData { get; set; } = new List<TelemetryDatum>();
}
