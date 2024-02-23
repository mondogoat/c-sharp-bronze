using System.Net;
using Newtonsoft.Json;
using RestSharp;
using FluentAssertions;
using Tests.Models;

namespace Tests.TestFixtures;

public class CreateTodoItemTests
{
    [Test]
    public void CreateTodoItemSuccess()
    {
        var endpoint = "http://localhost:8080/api/TodoItems";

        var client = new RestClient(endpoint);
        var request = new RestRequest();
        request.Method = Method.Post;

        var payload = new TodoItemModel
        {
            name = "test3",
            isComplete = false
        };
        request.AddJsonBody(payload);
        
        var response = client.Execute(request);
        var responseBody = JsonConvert.DeserializeObject<TodoItemModel>(response.Content);

        Console.WriteLine(response.Content);
        
        // Validate status code
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        responseBody.Should().NotBeNull();
        responseBody.id.Should().NotBe(null);
    }
}