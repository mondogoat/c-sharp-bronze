using System.Net;
using Newtonsoft.Json;
using RestSharp;
using FluentAssertions;
using Tests.Models;

namespace Tests.TestFixtures;

public class CreateTodoItemTests
{
    private readonly TestUtilities _testUtilities;

    public CreateTodoItemTests()
    {
        _testUtilities = new TestUtilities("http://localhost:8080/api");
    }

    [Test]
    public void CreateTodoItemSuccess()
    {
        var endpoint = "TodoItems";
        var payload = new TodoItemModel
        {
            id = 999,
            name = "test3",
            isComplete = false
        };
        var (responseBody, statusCode) = _testUtilities.ExecutePostRequest<TodoItemModel>(endpoint, payload);
        
        // Validate status code
        statusCode.Should().Be(HttpStatusCode.Created);
        responseBody.Should().NotBeNull();
        responseBody.id.Should().NotBe(null);
    }
}