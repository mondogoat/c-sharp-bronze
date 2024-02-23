using Newtonsoft.Json;
using RestSharp;
using FluentAssertions;
using Tests.Models;

namespace Tests.TestFixtures;

public class GetAllTodoTests
{
    [Test]
    public void GetAllTodoSuccess()
    {
        var endpoint = "http://localhost:8080/api/TodoItems";

        var client = new RestClient(endpoint);
        var request = new RestRequest();
        request.Method = Method.Get;

        var response = client.Execute(request);
        var responseBody = JsonConvert.DeserializeObject<List<TodoItemModel>>(response.Content);
        
        // Validate status code
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        responseBody.Should().NotBeNullOrEmpty();
    }
}