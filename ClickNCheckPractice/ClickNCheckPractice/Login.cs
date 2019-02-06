

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace ClickNCheckPractice
{
    public class TodoCreateModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("n");
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
    }

    public static class Login
    {
        //[FunctionName("Login")]
        //public static async Task<IActionResult> CreateTodo([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "todo2")]HttpRequest req, [Table("todos", ConnectionInfo = "AzureWebJobsStorage")] IAsyncCollector<UsersTableEntity> todoTable,
        //TraceWriter log)
        //{
        //    log.Info("Creating a new todo list item");
        //    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        //    var input = JsonConvert.DeserializeObject<TodoCreateModel>(requestBody);

        //    var user = new Users() {
        //        FirstName = input.FirstName,
        //        LastName = input.LastName,
        //        Username = input.Username,
        //        Password = input.Password,
        //        Salt = input.Salt
        //    };
        //    await todoTable.AddAsync(user.ToTableEntity());
        //    return new OkObjectResult(user);
        //}

        [FunctionName("Login")]
        public static IActionResult Run(
      [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, [Table("Users")] out Users ud, ILogger log)
            {
                log.LogInformation("C# HTTP trigger function processed a request.");

                string text = req.Query["text"];
                string firstName = req.Query["FirstName"];
                string lastName = req.Query["LastName"];
                string username = req.Query["Username"];
                string password = req.Query["Password"];
                string salt = req.Query["Salt"];
                string requestBody = new StreamReader(req.Body).ReadToEnd();
                dynamic data = JsonConvert.DeserializeObject(requestBody);
                text = text ?? data?.text;

                ud = new Users
                {
                    PartitionKey = "1",
                    RowKey = DateTime.Now.Ticks.ToString(),
                    FirstName = firstName,
                    LastName = lastName,
                    Username = username,
                    Password = password,
                    Salt = salt

                };

                return text != null
                    ? (ActionResult)new OkObjectResult($"Hello, {text}")
                    : new BadRequestObjectResult("Please pass some text on the query string or in the request body");
        }
    }
}
