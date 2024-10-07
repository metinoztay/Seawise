using System;
using System.Collections.Generic;

namespace Seawise.Models;

public partial class MaintenanceRecord
{
    public int MaintenanceRecordId { get; set; }

    public int ShipEquipmentId { get; set; }

    public string Description { get; set; } = null!;

    public bool Status { get; set; }

    public DateTime Time { get; set; }

    public virtual ICollection<MaintenanceParticipant> MaintenanceParticipants { get; set; } = new List<MaintenanceParticipant>();

    public virtual ShipEquipment ShipEquipment { get; set; } = null!;
}
