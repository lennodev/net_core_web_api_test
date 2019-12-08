using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using net_core_web_api_test.Models;

namespace net_core_web_api_test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public PersonController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Person> Get()
        {
            Person personObj = null;
            List<Person> personList = new List<Person>();
            try 
            { 
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

                builder.ConnectionString="Server=tcp:????.database.windows.net,1433;Initial Catalog=dev_db_mssql;Persist Security Info=False;User ID=???;Password=????;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
         
                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    
                    connection.Open();       
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT id, lastName, firstName, address, city from [Persons]; ");
                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                personObj = new Person();
                                personObj.id = reader.GetInt32(0);
                                personObj.lastName = reader.GetString(1);
                                personObj.firstName = reader.GetString(2);
                                personObj.address = reader.GetString(3);
                                personObj.city = reader.GetString(4);

                                personList.Add(personObj);
                            }
                        }
                    }                    
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            
            return Enumerable.Range(1, personList.Count).Select(
                index => personList[index-1]
            ).ToArray();
        }
    }
}
