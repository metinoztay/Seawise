using System;
using System.Collections.Generic;

namespace Seawise.Models;

public partial class EmployeePosition
{
    public int EmployeePositionId { get; set; }

    public int EmployeeDepartmentId { get; set; }

    public string PositionName { get; set; } = null!;

    public virtual EmployeeDepartment EmployeeDepartment { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<PositionPermission> PositionPermissions { get; set; } = new List<PositionPermission>();
}
