using Seawise.Models;

namespace Seawise.Data
{
    public static class Authorities
    {
        private static readonly Db8536Context _context = new Db8536Context();

        static List<string> adminAuths = new List<string> { "ALL"};
        static List<string> generalManagerAuths = new List<string> { 
            "OWNER","OWNERADD","OWNERUPDATE",
            "SHİP","SHİPADD","SHİPUPDATE",
            "EMPLOYEE","EMPLOYEEADD","EMPLOYEEUPDATE",
            "MAİNTENANCE","MAİNTENANCEADD","MAİNTENANCEUPDATE",
            "İNSPECTİON","İNSPECTİONADD","İNSPECTİONUPDATE",
            "EQUİPMENT","EQUİPMENTADD","EQUİPMENTUPDATE",
            "FİNDİNGDELETE",
            "PARTİCİPANTADD"
        };
        static List<string> inspectionManagerAuths = new List<string> { 
            "OWNER", "OWNERADD", "OWNERUPDATE",
            "SHİP", "SHİPADD", "SHİPUPDATE",
            "EMPLOYEE",
            "İNSPECTİON","İNSPECTİONADD","İNSPECTİONUPDATE",
            "EQUİPMENT","EQUİPMENTADD","EQUİPMENTUPDATE"
        };
        static List<string> inspectionStaffAuths = new List<string> { 
            "OWNER", 
            "SHİP",
            "İNSPECTİON","İNSPECTİONUPDATE",
            "MAİNTENANCE",
            "EQUİPMENT"
        };
        static List<string> maintenanceManagerAuths = new List<string> { 
            "OWNER", "OWNERADD", "OWNERUPDATE", 
            "SHİP", "SHİPADD", "SHİPUPDATE", 
            "EMPLOYEE",
            "MAİNTENANCE", "MAİNTENANCEADD", "MAİNTENANCEUPDATE",
            "EQUİPMENT","EQUİPMENTADD","EQUİPMENTUPDATE"
             };
        static List<string> maintenanceStaffAuths = new List<string> { 
            "OWNER",
            "SHİP",
            "İNSPECTİON",
            "MAİNTENANCE","MAİNTENANCEUPDATE",
            "EQUİPMENT","EQUİPMENTUPDATE"};
        static List<string> hrAuths = new List<string> { 
            "EMPLOYEE", "EMPLOYEEADD", "EMPLOYEEUPDATE"
        };
        public static bool CheckAuth(int employeePositionId, string authClass)
        {
            bool auth = false;
            authClass = authClass.ToUpper();

            switch (employeePositionId)
            {
                case 1: //admin
                    auth = true;
                    break;
                case 2: //generalMenager
                    auth = true;
                    break;
                case 4: //inspectionManager
                    auth = inspectionManagerAuths.Contains(authClass);
                    break;
                case 5: //inspectionStaff
                    auth = inspectionStaffAuths.Contains(authClass);
                    break;
                case 7: //maintenanceManager
                    auth = maintenanceManagerAuths.Contains(authClass);
                    break;
                case 8: //maintenanceStaff
                    auth = maintenanceStaffAuths.Contains(authClass);
                    break;
                case 9: //humanResources
                    auth = hrAuths.Contains(authClass);
                    break;
                default:
                    auth = false;
                    break;
            }         
            return auth;
        }

        static public bool isParticipant(int employeeId,int recordId, string recordType)
        {
            bool isParticipant = false;

            if (recordType=="MAİNTENANCE")
            {
                isParticipant = _context.MaintenanceParticipants.Any(m => m.MaintenanceRecordId == recordId && m.EmployeeId == employeeId);
            }
            else if(recordType == "İNSPECTİON")
            {
                isParticipant = _context.InspectionParticipants.Any(m => m.InspectionRecordId == recordId && m.EmployeeId == employeeId);
            }

            return isParticipant;
        }
    }
}
