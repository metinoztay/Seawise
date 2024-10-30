using System;
using System.Collections.Generic;

namespace Seawise.Models;

public partial class ShipEquipment
{
    public int ShipEquipmentId { get; set; }

    public int ShipEquipmentCategoryId { get; set; }

    public int ShipId { get; set; }

    public string EquipmentName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string PhotoUrl { get; set; } = null!;

    public DateOnly AdditionDate { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<MaintenanceRecord> MaintenanceRecords { get; set; } = new List<MaintenanceRecord>();

    public virtual Ship Ship { get; set; } = null!;

    public virtual ShipEquipmentCategory ShipEquipmentCategory { get; set; } = null!;
}
