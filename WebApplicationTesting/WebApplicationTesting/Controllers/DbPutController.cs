using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;

namespace WebApplicationTesting.Controllers
{
     
        [ApiController]
        [Route("[controller]")]
        public class DbPutController : ControllerBase
        {
            private readonly string ConnectionString = "Data Source=192.168.169.55:1600/IND83;Connection Lifetime=0;Pooling=yes;Max Pool Size=1024;Min Pool Size=1;User ID=MARS;password=mars;";

            [HttpPut("UpdateData")]
            public IActionResult UpdateData()
            {
                using (OracleConnection objcon = new OracleConnection(ConnectionString))
                using (OracleCommand oracleCommand = new OracleCommand("update EMPLOYEE set emp_name = 'ramesh' where emp_name = 'suresh' ", objcon))
                {
                    try
                    {
                        Console.WriteLine("Before connection string execution...");
                        objcon.Open();
                        Console.WriteLine("After connection string execution...");

                      
                        int rowsAffected = oracleCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return Ok("Data updated successfully.");
                        }
                        else
                        {
                            return NotFound("No data found to update.");
                        }
                    }
                    catch (Exception e)
                    {
                        return StatusCode(500, $"Internal server error: {e.Message}");
                    }
                }
            }
        }
    }

