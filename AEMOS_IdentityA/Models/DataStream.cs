using System;
using System.Collections.Generic;

namespace AEMOS_IdentityA.Models;

public partial class DataStream
{
    public long Id { get; set; }

    public string? Value { get; set; }

    public long? TelemetryDataId { get; set; }

    public DateTime? RecievedAt { get; set; }

    public virtual TelemetryDatum? TelemetryData { get; set; }
}
