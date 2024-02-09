 

using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using OracleInternal.SqlAndPlsqlParser.LocalParsing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
 
using System;
using System.Collections.Generic;
using WebApplicationTesting;
namespace WebApplicationTesting.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class StoredProcedureController : ControllerBase
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
                    Console.WriteLine("before connection string execution..");
                    objcon.Open();
                    Console.WriteLine("Afte connection string....");
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
        public class DataModel
        {
            public int EMP_ID { get; set; }
            public string EMP_NAME { get; set; }
            public int DEP_ID { get; set; }
            public DateTime DATE_OF_JOIN { get; set; }
        }


    }


}

