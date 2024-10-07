using System;
using System.Collections.Generic;

namespace Seawise.Models;

public partial class InspectionParticipant
{
    public int InspectionParticipantId { get; set; }

    public int InspectionRecordId { get; set; }

    public int EmployeeId { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual InspectionRecord InspectionRecord { get; set; } = null!;
}
