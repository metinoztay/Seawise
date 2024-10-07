using System;
using System.Collections.Generic;

namespace Seawise.Models;

public partial class InspectionType
{
    public int InspectionTypeId { get; set; }

    public string TypeName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<InspectionCriteria> InspectionCriteria { get; set; } = new List<InspectionCriteria>();

    public virtual ICollection<InspectionFinding> InspectionFindings { get; set; } = new List<InspectionFinding>();

    public virtual ICollection<InspectionRecord> InspectionRecords { get; set; } = new List<InspectionRecord>();
}
