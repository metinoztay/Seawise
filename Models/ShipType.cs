using System;
using System.Collections.Generic;

namespace Seawise.Models;

public partial class ShipType
{
    public int ShipTypeId { get; set; }

    public string ShipTypeName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<Ship> Ships { get; set; } = new List<Ship>();
}
