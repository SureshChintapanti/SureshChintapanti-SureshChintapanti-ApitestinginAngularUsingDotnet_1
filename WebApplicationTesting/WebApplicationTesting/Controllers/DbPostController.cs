using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System;

namespace WebApplicationTesting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DbPostController : ControllerBase
    {
        private readonly string ConnectionString = "Data Source=192.168.169.55:1600/IND83;Connection Lifetime=0;Pooling=yes;Max Pool Size=1024;Min Pool Size=1;User ID=MARS;password=mars;";

        [HttpPost("AddEmployee")]
        public IActionResult AddEmployee(DataModel newEmployee)
        {
            if (newEmployee == null)
            {
                return BadRequest("Invalid input data");
            }

            using (OracleConnection objcon = new OracleConnection(ConnectionString))
            using (OracleCommand oracleCommand = new OracleCommand("INSERT INTO EMPLOYEE (EMP_NAME, DEP_ID, DATE_OF_JOIN) VALUES (:EMP_NAME, :DEP_ID, :DATE_OF_JOIN)", objcon))
            {
                try
                {
                    objcon.Open();

                    oracleCommand.Parameters.Add(":EMP_NAME", OracleDbType.Varchar2).Value = newEmployee.EMP_NAME;
                    oracleCommand.Parameters.Add(":DEP_ID", OracleDbType.Int32).Value = newEmployee.DEP_ID;

                    oracleCommand.Parameters.Add(":DATE_OF_JOIN", OracleDbType.Date).Value = newEmployee.DATE_OF_JOIN.Date;

                    int rowsAffected = oracleCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return Ok(new { msg = "Employee added successfully" });
                    }
                    else
                    {
                        return StatusCode(500, "Failed to add employee");
                    }
                }
                catch (Exception e)
                {
                    return StatusCode(500, $"Internal server error: {e.Message}");
                }
                finally
                {
                    objcon.Close();
                }
            }
        }

        public class DataModel
        {
            public string EMP_NAME { get; set; }
            public int DEP_ID { get; set; }
            public DateTime DATE_OF_JOIN { get; set; }
        }
    }
}


/*using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System;

namespace WebApplicationTesting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DbPostController : ControllerBase
    {
        private readonly string ConnectionString = "Data Source=192.168.169.55:1600/IND83;Connection Lifetime=0;Pooling=yes;Max Pool Size=1024;Min Pool Size=1;User ID=MARS;password=mars;";

        [HttpPost("AddEmployee")]
        public IActionResult AddEmployee(EmployeeInputModel newEmployee)
        {
            if (newEmployee == null)
            {
                return BadRequest("Invalid input data");
            }

            using (OracleConnection objcon = new OracleConnection(ConnectionString))
            {
                try
                {
                    objcon.Open();

                    // Query to retrieve DEP_ID based on DEP_NAME
                    string query = "SELECT DEP_ID FROM DEPARTMENT WHERE DEP_NAME = :DEP_NAME";
                    using (OracleCommand depCommand = new OracleCommand(query, objcon))
                    {
                        depCommand.Parameters.Add(":DEP_NAME", OracleDbType.Varchar2).Value = newEmployee.DEP_NAME;
                        object depIdResult = depCommand.ExecuteScalar();

                        if (depIdResult == null)
                        {
                            return StatusCode(400, "Department does not exist");
                        }

                        int depId = Convert.ToInt32(depIdResult);

                        // Insert into Employee table using retrieved DEP_ID
                        query = "INSERT INTO EMPLOYEE (EMP_NAME, DEP_ID, DATE_OF_JOIN) VALUES (:EMP_NAME, :DEP_ID, :DATE_OF_JOIN)";
                        using (OracleCommand empCommand = new OracleCommand(query, objcon))
                        {
                            empCommand.Parameters.Add(":EMP_NAME", OracleDbType.Varchar2).Value = newEmployee.EMP_NAME;
                            empCommand.Parameters.Add(":DEP_ID", OracleDbType.Int32).Value = depId;
                            empCommand.Parameters.Add(":DATE_OF_JOIN", OracleDbType.Date).Value = newEmployee.DATE_OF_JOIN.Date;

                            int rowsAffected = empCommand.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                return Ok(new { msg = "Employee added successfully" });
                            }
                            else
                            {
                                return StatusCode(500, "Failed to add employee");
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    return StatusCode(500, $"Internal server error: {e.Message}");
                }
                finally
                {
                    objcon.Close();
                }
            }
        }

        public class EmployeeInputModel
        {
            public string EMP_NAME { get; set; }
            public string DEP_NAME { get; set; }
            public DateTime DATE_OF_JOIN { get; set; }
        }
    }
}
*/



