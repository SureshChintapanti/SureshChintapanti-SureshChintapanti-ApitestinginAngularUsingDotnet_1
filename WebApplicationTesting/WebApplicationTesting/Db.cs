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
namespace WebApplicationTesting
{
    public class Db
    {
        static void Main(string[] args)
        {
            string ConnectionString = "Data Source=192.168.169.55:1600/IND83;Connection Lifetime=0;Pooling=yes;Max Pool Size=1024;Min Pool Size=1;User ID=MARS;password=mars;";
            //ExecuteQuery(ConnectionString);

            ExecuteNameQuery(ConnectionString);

        }
        private static void ExecuteNameQuery(string ConnectionString)
        {
            OracleConnection objcon = new OracleConnection(ConnectionString);
            OracleCommand oracleCommand = new OracleCommand();
            oracleCommand.Connection = objcon;
            oracleCommand.CommandText = "   SELECT NAME ,PERCENTAGE FROM CLASS_DETAILS";
            OracleDataReader reader = null;
            try
            {
                objcon.Open();
                reader = oracleCommand.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine("___________________");
                    Console.WriteLine(reader["NAME"]);
                    Console.WriteLine(reader["PERCENTAGE"]);

                    Console.WriteLine("___________________");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                reader.Close();
                objcon.Close();
            }





        }
    }


}





 
     
      
  