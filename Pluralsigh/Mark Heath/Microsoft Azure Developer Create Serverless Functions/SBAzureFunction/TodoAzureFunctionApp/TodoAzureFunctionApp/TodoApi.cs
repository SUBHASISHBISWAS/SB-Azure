using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using Azure.Data.Tables;
using Azure;
using System;
using System.Collections.Generic;

namespace TodoAzureFunctionApp
{
    public static class TodoApi
    {
        private const string Route = "todo";
        private const string TableName = "todos";
        private const string PartitionKey = "TODO";


        /* This is INmemory Version
         *
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

        */

        [FunctionName("CreateTodo")]
        public static async Task<IActionResult> CreateTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = Route)] HttpRequest req,
            [Table(TableName, Connection = "AzureWebJobsStorage")] IAsyncCollector<TodoTableEntity> todoTable,
            ILogger log)
        {
            log.LogInformation("Creating a new todo list Item");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var input = JsonConvert.DeserializeObject<TodoCreateModel>(requestBody);

            var todo = new Todo() { TaskDescription = input.TaskDescription };
            await todoTable.AddAsync(todo.ToTableEntity());

            return new OkObjectResult(todo);
        }

        //Funtion to get All Todo Items
        [FunctionName("GetTodos")]
        public static async Task<IActionResult> GetTodos(
                                  [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Route)] HttpRequest req,
                                                        
                                  [Table(TableName, Connection = "AzureWebJobsStorage")] TableClient todoTable,
                                                                                         
                                  ILogger log)
        {
            log.LogInformation("Getting todo list items");
            var pages =  todoTable.QueryAsync<TodoTableEntity>().AsPages();

            var todolist = new List<Todo>();
            await foreach (Page<TodoTableEntity> page in pages)
            {
                Console.WriteLine("This is a new page!");
                todolist = page.Values.Select(Mappings.ToTodo).ToList();
                
            }

            return new OkObjectResult(todolist);
        }

        //Function to get Todo Item by Id
        [FunctionName("GetTodoById")]
        public static IActionResult GetTodoById(
                                  [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = Route + "/{id}")] HttpRequest req,
                                                                   
                                  [Table(TableName, "TODO", "{id}", Connection = "AzureWebJobsStorage")] TodoTableEntity todo,
                                                                                                    
                                  ILogger log, string id)
        {
            log.LogInformation("Getting todo item by id");
            if (todo == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(todo.ToTodo());
        }

        //Function to update Todo Item by Id
        [FunctionName("UpdateTodo")]
        public static async Task<IActionResult> UpdateTodo(
                                             [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = Route + "/{id}")] HttpRequest req,
                                                                                                               
                                             [Table(TableName, Connection = "AzureWebJobsStorage")] TableClient todoTable,
                                                                                                                                                                                                                  
                                             ILogger log, string id)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var updated = JsonConvert.DeserializeObject<TodoUpdateModel>(requestBody);
            TodoTableEntity existingRow;
            try
            {
                var findResult = await todoTable.GetEntityAsync<TodoTableEntity>(PartitionKey, id);
                existingRow = findResult.Value;
            }
            catch (RequestFailedException e) when (e.Status == 404)
            {
                return new NotFoundResult();
            }

            existingRow.IsCompleted = updated.IsCompleted;
            if (!string.IsNullOrEmpty(updated.TaskDescription))
            {
                existingRow.TaskDescription = updated.TaskDescription;
            }

            await todoTable.UpdateEntityAsync(existingRow, existingRow.ETag, TableUpdateMode.Replace);

            return new OkObjectResult(existingRow.ToTodo());
        }

        //Function to delete Todo Item by Id
        [FunctionName("DeleteTodo")]
        public static async Task<IActionResult> DeleteTodo(
             [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = Route + "/{id}")] HttpRequest req,
             [Table(TableName, Connection = "AzureWebJobsStorage")] TableClient todoTable,
              ILogger log, string id)
        {
            
            
            try
            {
                var deleteResult = await todoTable.DeleteEntityAsync("TODO", id,  ETag.All);
            }
            catch (RequestFailedException e) when (e.Status == 404)
            {
                return new NotFoundResult();
            }
            return new OkResult();
        }




    }
        
}
