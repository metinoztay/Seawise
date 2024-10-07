using System;
using System.Collections.Generic;

namespace Seawise.Models;

public partial class MaintenanceParticipant
{
    public int MaintenanceParticipantId { get; set; }

    public int MaintenanceRecordId { get; set; }

    public int EmployeeId { get; set; }

    public DateTime? StartedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual MaintenanceRecord MaintenanceRecord { get; set; } = null!;
}
