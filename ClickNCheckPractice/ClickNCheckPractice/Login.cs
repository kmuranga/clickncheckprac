using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;

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
        [FunctionName("Login")]
        public static async Task<IActionResult> CreateTodo([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "todo2")]HttpRequest req, [Table("todos", Connection = "AzureWebJobsStorage")] IAsyncCollector<UsersTableEntity> todoTable,
        TraceWriter log)
        {
            log.Info("Creating a new todo list item");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<TodoCreateModel>(requestBody);

            var user = new Users() {
                FirstName = input.FirstName,
                LastName = input.LastName,
                Username = input.Username,
                Password = input.Password,
                Salt = input.Salt
            };
            await todoTable.AddAsync(user.ToTableEntity());
            return new OkObjectResult(user);
        }
    }
}
