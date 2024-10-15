using System;
using System.Collections.Generic;

namespace Seawise.Models;

public partial class ShipOwner
{
    public int ShipOwnerId { get; set; }

    public string Name { get; set; } = null!;

    public bool IsPerson { get; set; }

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string CountryCode { get; set; } = null!;

    public string PhotoUrl { get; set; } = null!;

    public virtual Country CountryCodeNavigation { get; set; } = null!;

    public virtual ICollection<Ship> Ships { get; set; } = new List<Ship>();
}
