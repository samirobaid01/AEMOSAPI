using System;
using System.Collections.Generic;

namespace AEMOS_IdentityA.Models;

public partial class State
{
    public long Id { get; set; }

    public long? DeviceId { get; set; }

    public string? Name { get; set; }

    public bool? IsActive { get; set; }

    public virtual Device? Device { get; set; }
}
