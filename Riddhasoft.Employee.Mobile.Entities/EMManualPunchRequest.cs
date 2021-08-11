namespace Riddhasoft.Employee.Mobile.Entities
{
    public class EMManualPunchRequest
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public int EmployeeId { get; set; }
        public string Image { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Altitude { get; set; }
        public string Remark { get; set; }
        public string Address { get; set; }
    }
}
