using System;
using System.Collections.Generic;

namespace Seawise.Models;

public partial class EmployeeDepartment
{
    public int EmployeeDepartmentId { get; set; }

    public string DepartmentName { get; set; } = null!;

    public virtual ICollection<EmployeePosition> EmployeePositions { get; set; } = new List<EmployeePosition>();
}
