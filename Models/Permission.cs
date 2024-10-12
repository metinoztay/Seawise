using System;
using System.Collections.Generic;

namespace Seawise.Models;

public partial class Permission
{
    public int PermissionId { get; set; }

    public string PermissionName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<PositionPermission> PositionPermissions { get; set; } = new List<PositionPermission>();
}
