namespace RTech.Demo.Models
{
    public class EmpSearchViewModel
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Designation { get; set; }
        public int? DesignationId { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string Photo { get; set; }
        public string ReportingManagerName { get; set; }
        public int? ReportingManagerId { get; set; }
    }
}