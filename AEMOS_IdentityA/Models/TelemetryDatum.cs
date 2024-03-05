using System;
using System.Collections.Generic;

namespace AEMOS_IdentityA.Models;

public partial class TelemetryDatum
{
    public long Id { get; set; }

    public string? VariableName { get; set; }

    public string? Datatype { get; set; }

    public long? DeviceId { get; set; }

    public virtual ICollection<DataStream> DataStreams { get; set; } = new List<DataStream>();

    public virtual Device? Device { get; set; }
}
