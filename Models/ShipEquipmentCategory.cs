using System;
using System.Collections.Generic;

namespace Seawise.Models;

public partial class ShipEquipmentCategory
{
    public int ShipEquipmentCategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<ShipEquipment> ShipEquipments { get; set; } = new List<ShipEquipment>();
}
