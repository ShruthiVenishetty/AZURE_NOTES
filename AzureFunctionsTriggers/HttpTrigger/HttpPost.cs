using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;

namespace HttpTrigger
{
    public static class HttpPost
    {
        [FunctionName("HttpPost")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            // We then use the JsonConvert class to convert the string of the request body to a Course object
            Movie data = JsonConvert.DeserializeObject<Movie>(requestBody);



            string _connection_string = Environment.GetEnvironmentVariable($"ConnectionStrings:SQLConnectionString");
            string _statement = "INSERT INTO Movies(Name,Language) VALUES(@param2,@param3)";
            SqlConnection _connection = new SqlConnection(_connection_string);
            _connection.Open();




            // Here we create a parameterized query to insert the data into the database
            using (SqlCommand _command = new SqlCommand())
            {
                _command.Connection = _connection;
                _command.CommandText = _statement;


                _command.Parameters.AddWithValue("@param2", data.Name);
                _command.Parameters.AddWithValue("@param3", data.Language);
                _command.CommandType = CommandType.Text;
                _command.ExecuteNonQuery();

            }

            return new OkObjectResult("Movie added");
        }
    }
}
