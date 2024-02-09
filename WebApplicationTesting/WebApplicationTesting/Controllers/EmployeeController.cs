using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;


namespace WebApplicationTesting.Controllers
{

    namespace WebApplicationTesting.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class EmployeeController : ControllerBase
        {
            private readonly string ConnectionString = "Data Source=192.168.169.55:1600/IND83;Connection Lifetime=0;Pooling=yes;Max Pool Size=1024;Min Pool Size=1;User ID=MARS;password=mars;";

            [HttpPost("AddEmployee")]
            public IActionResult AddEmployee(EmployeeRequest request)
            {
                using (OracleConnection objcon = new OracleConnection(ConnectionString))
                using (OracleCommand oracleCommand = new OracleCommand("ADD_EMPLOYEE_INFORMATION", objcon))
                {
                    try
                    {
                        objcon.Open();
 
                        oracleCommand.CommandType = CommandType.StoredProcedure;

                
                        oracleCommand.Parameters.Add("IO_EMP_ID", OracleDbType.Int32).Direction = ParameterDirection.InputOutput;
                        oracleCommand.Parameters.Add("IP_EMP_NAME", OracleDbType.Varchar2).Value = request.EmployeeName;
                        oracleCommand.Parameters.Add("IP_DEP_ID", OracleDbType.Int32).Value = request.DepartmentId;
                        oracleCommand.Parameters.Add("IP_DATE_OF_JOIN", OracleDbType.Date).Value = request.DateOfJoin;
                        oracleCommand.Parameters.Add("IP_OPERATION", OracleDbType.Char).Value = 'I';

              
                        oracleCommand.ExecuteNonQuery();

     
                        OracleDecimal oracleEmpId = (OracleDecimal)oracleCommand.Parameters["IO_EMP_ID"].Value;
                        int empId = oracleEmpId.ToInt32();

                        return Ok($"Employee added successfully with ID: {empId}");
                    }
                    catch (Exception e)
                    {
                        return StatusCode(500, $"Internal server error: {e.Message}");
                    }
                }
            }


            

            public class EmployeeRequest
            {
                public string EmployeeName { get; set; }
                public int DepartmentId { get; set; }
                public DateTime DateOfJoin { get; set; }
            }

             

            [HttpGet("GetData")]
            public IActionResult GetData()
            {
                List<DataModel> data = new List<DataModel>();

                using (OracleConnection objcon = new OracleConnection(ConnectionString))
                using (OracleCommand oracleCommand = new OracleCommand("get_employee_information", objcon))
                {
                    try
                    {
                        objcon.Open();
 
                        oracleCommand.CommandType = System.Data.CommandType.StoredProcedure;

      
                        oracleCommand.Parameters.Add("op_outrec", OracleDbType.RefCursor, System.Data.ParameterDirection.Output);

 
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
            


            [HttpPut("UpdateEmployee/{empId}")]
            public IActionResult UpdateEmployee(int empId, Employee employee)
            {
                using (OracleConnection objcon = new OracleConnection(ConnectionString))
                using (OracleCommand oracleCommand = new OracleCommand("ADD_EMPLOYEE_INFORMATION", objcon))
                {
                    try
                    {
                        objcon.Open();
 
                        oracleCommand.CommandType = System.Data.CommandType.StoredProcedure;

                       
                        oracleCommand.Parameters.Add("IO_EMP_ID", OracleDbType.Int32).Value = empId;
                        oracleCommand.Parameters.Add("IP_EMP_NAME", OracleDbType.Varchar2).Value = employee.EmployeeName;
                        oracleCommand.Parameters.Add("IP_DEP_ID", OracleDbType.Int32).Value = employee.DepartmentId;
                        oracleCommand.Parameters.Add("IP_DATE_OF_JOIN", OracleDbType.Date).Value = employee.DateOfJoin;
                        oracleCommand.Parameters.Add("IP_OPERATION", OracleDbType.Char).Value = 'U';

                 
                        oracleCommand.ExecuteNonQuery();

                        return Ok($"Employee with ID {empId} updated successfully");
                    }
                    catch (Exception e)
                    {
                        return StatusCode(500, $"Internal server error: {e.Message}");
                    }
                }
            }

            [HttpDelete("DeleteEmployee/{empId}")]
            public IActionResult DeleteEmployee(int empId)
            {
                using (OracleConnection objcon = new OracleConnection(ConnectionString))
                using (OracleCommand oracleCommand = new OracleCommand("ADD_EMPLOYEE_INFORMATION", objcon))
                {
                    try
                    {
                        objcon.Open();

                     
                        oracleCommand.CommandType = System.Data.CommandType.StoredProcedure;
                         
                        oracleCommand.Parameters.Add("IO_EMP_ID", OracleDbType.Int32).Value = empId;
                        oracleCommand.Parameters.Add("IP_EMP_NAME", OracleDbType.Varchar2).Value = DBNull.Value;
                        oracleCommand.Parameters.Add("IP_DEP_ID", OracleDbType.Int32).Value = DBNull.Value;
                        oracleCommand.Parameters.Add("IP_DATE_OF_JOIN", OracleDbType.Date).Value = DBNull.Value;
                        oracleCommand.Parameters.Add("IP_OPERATION", OracleDbType.Char).Value = 'D';
 
                        oracleCommand.ExecuteNonQuery();

                        return Ok($"Employee with ID {empId} deleted successfully");
                    }
                    catch (Exception e)
                    {
                        return StatusCode(500, $"Internal server error: {e.Message}");
                    }
                }
            }


           
               
      
                [HttpDelete("DeleteEmployeeByID/{empId}")]
                public IActionResult DeleteEmployeeByID(int empId)
                {
                    using (OracleConnection objcon = new OracleConnection(ConnectionString))
                    using (OracleCommand oracleCommand = new OracleCommand($"DELETE FROM Employee WHERE EMP_ID = :empId", objcon))
                    {
                        try
                        {
                            objcon.Open();

                            oracleCommand.Parameters.Add(":empId", OracleDbType.Int32).Value = empId;

                            int rowsAffected = oracleCommand.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                return Ok($"Employee with ID {empId} deleted successfully");
                            }
                            else
                            {
                                return NotFound($"Employee with ID {empId} not found");
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



    }




 

 
    

 
