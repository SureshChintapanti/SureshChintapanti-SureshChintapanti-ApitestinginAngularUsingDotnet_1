 using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;



namespace WebApplicationTesting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExampleEmployeeController : ControllerBase
    {
        private List<Employee> AllEmployees()
        {
            List<Employee> lstEmployee = new List<Employee>();

            lstEmployee.Add(new Employee { EmployeeId = 1, DateofJoining = DateTime.Now, Name = "Shan" });
            lstEmployee.Add(new Employee { EmployeeId = 2, DateofJoining = DateTime.Now, Name = "Suresh" });
            lstEmployee.Add(new Employee { EmployeeId = 3, DateofJoining = DateTime.Now, Name = "Sandhya" });

            return lstEmployee;
        }

        [HttpGet]
        public List<Employee> GetEmployees()
        {
            return AllEmployees();
        }

        [HttpGet("{id}")]
        public Employee GetEmployees(long id)
        {
            return AllEmployees().Find(s => s.EmployeeId == id);
        }

        [HttpPost]
        public long AddEmployee(Employee newEmployee)
        {
            AllEmployees().Add(newEmployee);
            return newEmployee.EmployeeId;
        }

    }
}
