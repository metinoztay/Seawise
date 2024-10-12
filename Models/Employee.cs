using System;
using System.Collections.Generic;

namespace Seawise.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public int EmployeePositionId { get; set; }

    public string InternalEmployeeCode { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string PhotoUrl { get; set; } = null!;

    public int Salary { get; set; }

    public bool IsActiveWorker { get; set; }

    public virtual EmployeePosition EmployeePosition { get; set; } = null!;

    public virtual ICollection<InspectionParticipant> InspectionParticipants { get; set; } = new List<InspectionParticipant>();

    public virtual ICollection<MaintenanceParticipant> MaintenanceParticipants { get; set; } = new List<MaintenanceParticipant>();
}
