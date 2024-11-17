namespace Seawise.Data
{
    public class EmployeeDTO
    {
            public int EmployeeId { get; set; }
            public int EmployeePositionId { get; set; }
            public string Phone { get; set; } = null!;
            public string Email { get; set; } = null!;
            public int Salary { get; set; }
            public string? LeaveDate { get; set; } // JSON'da string olarak alacağız
            public string PhotoUrl { get; set; } = null!;
        

    }
}
