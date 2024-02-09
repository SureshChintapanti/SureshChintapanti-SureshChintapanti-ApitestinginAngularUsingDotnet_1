namespace WebApplicationTesting
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public DateTime DateofJoining { get; set; }
        public object EmployeeName { get; internal set; }
        public object DepartmentId { get; internal set; }
        public object DateOfJoin { get; internal set; }
    }
}
