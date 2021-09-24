using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace HttpTrigger
{
    public static class HttpGet
    {
        [FunctionName("HttpGet")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            List<Movie> _lst = new List<Movie>();
            string _connection_string = Environment.GetEnvironmentVariable($"ConnectionStrings:SQLConnectionString");


            string _statement = "SELECT Id,Name,Language from Movies";
            // We first establish a connection to the database
            SqlConnection _connection = new SqlConnection(_connection_string);
            _connection.Open();
            SqlCommand _sqlcommand = new SqlCommand(_statement, _connection);
            // Using the SqlDataReader class , we can get all the rows from the table
            using (SqlDataReader _reader = _sqlcommand.ExecuteReader())
            {
                while (_reader.Read())
                {
                    Movie _course = new Movie()
                    {
                        Id = _reader.GetInt32(0),
                        Name = _reader.GetString(1),
                        Language = _reader.GetString(2)
                    };



                    _lst.Add(_course);
                }
            }
            _connection.Close();
            // Return the HTTP status code of 200 OK and the list of courses
            return new OkObjectResult(_lst);
        }
    }
}
