using System;
using System.Collections.Generic;

namespace Seawise.Models;

public partial class PositionPermission
{
    public int PositionPermissionId { get; set; }

    public int PositionId { get; set; }

    public int PermissionId { get; set; }

    public virtual Permission Permission { get; set; } = null!;

    public virtual EmployeePosition Position { get; set; } = null!;
}
