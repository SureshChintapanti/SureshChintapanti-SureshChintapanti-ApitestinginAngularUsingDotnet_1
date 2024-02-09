using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System;

namespace WebApplicationTesting.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DbGetController : ControllerBase
    {
        private readonly string ConnectionString = "Data Source=192.168.169.55:1600/IND83;Connection Lifetime=0;Pooling=yes;Max Pool Size=1024;Min Pool Size=1;User ID=MARS;password=mars;";

        [HttpGet("GetData")]
        public IActionResult GetData()
        {
            try
            {
                using (OracleConnection objcon = new OracleConnection(ConnectionString))
                using (OracleCommand oracleCommand = new OracleCommand("SELECT * FROM EMPLOYEE", objcon))
                {
                    objcon.Open();

                    using (OracleDataReader reader = oracleCommand.ExecuteReader())
                    {
                        var data = new System.Collections.Generic.List<DataModel>();

                        while (reader.Read())
                        {
                            DataModel record = new DataModel
                            {
                                EMP_ID = Convert.ToInt32(reader["EMP_ID"]),
                                EMP_NAME = reader["EMP_NAME"].ToString(),
                                DEP_ID = Convert.ToInt32(reader["DEP_ID"]),
                                DATE_OF_JOIN = Convert.ToDateTime(reader["DATE_OF_JOIN"])
                            };

                            data.Add(record);
                        }

                        return Ok(data);
                    }
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }

       /* [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                using (OracleConnection objcon = new OracleConnection(ConnectionString))
                using (OracleCommand oracleCommand = new OracleCommand("SELECT * FROM EMPLOYEE WHERE EMP_ID = :id", objcon))
                {
                    oracleCommand.Parameters.Add("id", OracleDbType.Int32).Value = id;
                    objcon.Open();

                    using (OracleDataReader reader = oracleCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var record = new DataModel
                            {
                                EMP_ID = Convert.ToInt32(reader["EMP_ID"]),
                                EMP_NAME = reader["EMP_NAME"].ToString(),
                                DEP_ID = Convert.ToInt32(reader["DEP_ID"]),
                                DATE_OF_JOIN = Convert.ToDateTime(reader["DATE_OF_JOIN"])
                            };

                            return Ok(record);
                        }
                        else
                        {
                            return NotFound("Employee not found");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }
*/
        public class DataModel
        {
            public int EMP_ID { get; set; }
            public string EMP_NAME { get; set; }
            public int DEP_ID { get; set; }
            public DateTime DATE_OF_JOIN { get; set; }
        }



        [HttpGet("GetEmpById/{id}")]
        public IActionResult GetEmpById(int id)
        {
            try
            {
                using (OracleConnection objcon = new OracleConnection(ConnectionString))
                {
             
                    string query = "SELECT e.EMP_ID, e.EMP_NAME, e.DEP_ID, d.DEP_NAME AS DEPARTMENT_NAME, e.DATE_OF_JOIN " +
                                   "FROM EMPLOYEE e " +
                                   "INNER JOIN DEPARTMENT d ON e.DEP_ID = d.DEP_ID " +
                                   "WHERE e.EMP_ID = :id";

                    using (OracleCommand oracleCommand = new OracleCommand(query, objcon))
                    {
                        oracleCommand.Parameters.Add("id", OracleDbType.Int32).Value = id;
                        objcon.Open();

                        using (OracleDataReader reader = oracleCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var record = new EmployeeDataModel
                                {
                                    EMP_ID = Convert.ToInt32(reader["EMP_ID"]),
                                    EMP_NAME = reader["EMP_NAME"].ToString(),
                                    
                                    DEPARTMENT_NAME = reader["DEPARTMENT_NAME"].ToString(),
                                    DATE_OF_JOIN = Convert.ToDateTime(reader["DATE_OF_JOIN"])
                                };

                                return Ok(record);
                            }
                            else
                            {
                                return NotFound("Employee not found");
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }

        public class EmployeeDataModel
        {
            public int EMP_ID { get; set; }
            public string EMP_NAME { get; set; }
         
            public string DEPARTMENT_NAME { get; set; } // Add property for department name
            public DateTime DATE_OF_JOIN { get; set; }
        }



    }
}






/*namespace WebApplicationTesting.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DbController : ControllerBase
    {
        private readonly string ConnectionString = "Data Source=192.168.169.55:1600/IND83;Connection Lifetime=0;Pooling=yes;Max Pool Size=1024;Min Pool Size=1;User ID=MARS;password=mars;";

        [HttpGet("GetData")]
        public IActionResult GetData()
        {
            List<DataModel> data = new List<DataModel>();

            using (OracleConnection objcon = new OracleConnection(ConnectionString))
            using (OracleCommand oracleCommand = new OracleCommand("SELECT * FROM EMPLOYEE", objcon))
            {
                try
                {
                    objcon.Open();
                    using (OracleDataReader reader = oracleCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DataModel record = new DataModel
                            {
                                EMP_ID = Convert.ToInt32(reader["EMP_ID"]),
                                EMP_NAME = reader["EMP_NAME"].ToString(),
                                DEP_ID = Convert.ToInt32(reader["DEP_ID"]),
                                DATE_OF_JOIN = Convert.ToDateTime(reader["DATE_OF_JOIN"])
                            };

                            data.Add(record);
                        }
                    }

                    return Ok(data);
                }
                catch (Exception e)
                {
                    return StatusCode(500, $"Internal server error: {e.Message}");
                }
            }
        }
    }

    public class DataModel
    {
        public int EMP_ID { get; set; }
        public string EMP_NAME { get; set; }
        public int DEP_ID { get; set; }
        public DateTime DATE_OF_JOIN { get; set; }
    }
}
*/