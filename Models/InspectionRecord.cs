using System;
using System.Collections.Generic;

namespace Seawise.Models;

public partial class InspectionRecord
{
    public int InspectionRecordId { get; set; }

    public int ShipId { get; set; }

    public DateTime Time { get; set; }

    public int InspectionTypeId { get; set; }

    public virtual ICollection<InspectionFinding> InspectionFindings { get; set; } = new List<InspectionFinding>();

    public virtual ICollection<InspectionParticipant> InspectionParticipants { get; set; } = new List<InspectionParticipant>();

    public virtual InspectionType InspectionType { get; set; } = null!;

    public virtual Ship Ship { get; set; } = null!;
}
