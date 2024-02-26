using System.Net;
using Newtonsoft.Json;
using RestSharp;
using FluentAssertions;
using Tests.Models;

namespace Tests.TestFixtures;

public class CreateTodoItemTests: TestFixtureBase
{
    [Test]
    public void CreateTodoItemSuccess()
    {
        var endpoint = "http://localhost:8080/api/TodoItems";
        var (responseBody, statusCode) = ExecutePostRequest<TodoItemModel>(endpoint);
        
        // Validate status code
        statusCode.Should().Be(HttpStatusCode.Created);
        responseBody.Should().NotBeNull();
        responseBody.id.Should().NotBe(null);
    }
}