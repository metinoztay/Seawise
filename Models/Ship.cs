using System;
using System.Collections.Generic;

namespace Seawise.Models;

public partial class Ship
{
    public int ShipId { get; set; }

    public string Imonumber { get; set; } = null!;

    public string CountryCode { get; set; } = null!;

    public int ShipOwnerId { get; set; }

    public int ShipTypeId { get; set; }

    public string ShipName { get; set; } = null!;

    public string PhotoUrl { get; set; } = null!;

    public DateOnly LaunchDate { get; set; }

    public virtual Country CountryCodeNavigation { get; set; } = null!;

    public virtual ICollection<InspectionRecord> InspectionRecords { get; set; } = new List<InspectionRecord>();

    public virtual ICollection<ShipEquipment> ShipEquipments { get; set; } = new List<ShipEquipment>();

    public virtual ShipOwner ShipOwner { get; set; } = null!;

    public virtual ShipType ShipType { get; set; } = null!;
}
