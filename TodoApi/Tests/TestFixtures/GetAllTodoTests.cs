using Newtonsoft.Json;
using RestSharp;
using Tests.Models;

namespace Tests.TestFixtures;

public class GetAllTodoTests
{
    [Test]
    public void GetAllTodo()
    {
        var endpoint = "http://localhost:8080/api/TodoItems";

        var client = new RestClient(endpoint);
        var request = new RestRequest();
        request.Method = Method.Get;

        var response = client.Execute(request);
        
        // var responseBody = JsonConvert.DeserializeObject<List<TodoItemModel>>(response.Content);
        // var todoList = new TodoListModel
        // {
        //     TodoItems = responseBody
        // };
        //
        // var x = JsonConvert.SerializeObject(todoList);
        // Console.WriteLine(x);

        var responseBody = JsonConvert.DeserializeObject<List<TodoItemModel>>(response.Content);

        var x = JsonConvert.SerializeObject(responseBody);
        Console.WriteLine(x);

    }
}