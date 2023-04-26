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

namespace TodoAzureFunctionApp
{
    public static class TodoApi
    {
        static List<Todo> items = new List<Todo>();

        [FunctionName("CreateTodo")]
        public static async Task<IActionResult> CreateTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "todo")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Creating a new todo list Item");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<TodoCreateModel>(requestBody);

            var todo = new Todo() { TaskDescription = input.TaskDescription };
            items.Add(todo);

            return new OkObjectResult(todo);
        }

        //Funtion to get All Todo Items
        [FunctionName("GetAllTodo")]
        public static IActionResult GetAllTodo(
                       [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todo")] HttpRequest req,
                                  ILogger log)
        {
            log.LogInformation("Getting todo list items");
            return new OkObjectResult(items);
        }

        //Function to get Todo Item by Id
        [FunctionName("GetTodoById")]
        public static IActionResult GetTodoById(
                       [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todo/{id}")] HttpRequest req,
                                  ILogger log, string id)
        {
            log.LogInformation("Getting todo item by id");
            var todo = items.Find(i => i.Id == id);
            if (todo == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(todo);
        }

        //Function to update Todo Item by Id
        [FunctionName("UpdateTodo")]
        public static async Task<IActionResult> UpdateTodo(
                                  [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "todo/{id}")] HttpRequest req,
                                                                   ILogger log, string id)
        {
            var todo = items.Find(i => i.Id == id);
            if (todo == null)
            {
                return new NotFoundResult();
            }
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var updated = JsonConvert.DeserializeObject<TodoUpdateModel>(requestBody);
            todo.IsCompleted = updated.IsCompleted;
            return new OkObjectResult(todo);
        }

        //Function to delete Todo Item by Id
        [FunctionName("DeleteTodo")]
        public static IActionResult DeleteTodo(
                                  [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "todo/{id}")] HttpRequest req,
                                                                   ILogger log, string id)
        {
            var todo = items.Find(i => i.Id == id);
            if (todo == null)
            {
                return new NotFoundResult();
            }
            items.Remove(todo);
            return new OkResult();
        }

    }
}
