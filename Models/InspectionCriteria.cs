using System;
using System.Collections.Generic;

namespace Seawise.Models;

public partial class InspectionCriteria
{
    public int InspectionCriteriaId { get; set; }

    public int InspectionTypeId { get; set; }

    public string Description { get; set; } = null!;

    public virtual ICollection<InspectionFinding> InspectionFindings { get; set; } = new List<InspectionFinding>();

    public virtual InspectionType InspectionType { get; set; } = null!;
}
