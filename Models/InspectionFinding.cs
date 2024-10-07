using System;
using System.Collections.Generic;

namespace Seawise.Models;

public partial class InspectionFinding
{
    public int InspectionFindingId { get; set; }

    public int InspectionRecordId { get; set; }

    public int InspectionTypeId { get; set; }

    public int InspectionCriteriaId { get; set; }

    public byte Severity { get; set; }

    public string Description { get; set; } = null!;

    public virtual InspectionCriteria InspectionCriteria { get; set; } = null!;

    public virtual InspectionRecord InspectionRecord { get; set; } = null!;

    public virtual InspectionType InspectionType { get; set; } = null!;
}
