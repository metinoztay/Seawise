using System;
using System.Collections.Generic;

namespace Seawise.Models;

public partial class Country
{
    public string CountryCode { get; set; } = null!;

    public string CountryName { get; set; } = null!;

    public virtual ICollection<ShipOwner> ShipOwners { get; set; } = new List<ShipOwner>();

    public virtual ICollection<Ship> Ships { get; set; } = new List<Ship>();
}
