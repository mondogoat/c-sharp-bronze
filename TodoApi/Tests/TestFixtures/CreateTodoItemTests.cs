using System.Net;
using Newtonsoft.Json;
using RestSharp;
using FluentAssertions;
using Tests.Models;

namespace Tests.TestFixtures;

public class CreateTodoItemTests
{
    private readonly Helpers _helpers;

    public CreateTodoItemTests()
    {
        _helpers = new Helpers("http://localhost:8080/api");
    }

    [Test]
    public void CreateTodoItem_Success()
    {
        var endpoint = "TodoItems";
        var payload = new TodoItemModel
        {
            id = 999,
            name = "test3",
            isComplete = false
        };
        var (responseBody, statusCode) = _helpers.ExecutePostRequest<TodoItemModel>(endpoint, payload);
        
        // Validate status code
        statusCode.Should().Be(HttpStatusCode.Created);
        responseBody.Should().NotBeNull();
        responseBody.id.Should().NotBe(null);
    }

    [Test]
    public void CreateTodoItem_MissingId()
    {
        
    }

    [Test]
    public void CreateTodoItem_MissingName()
    {
        
    }

    [Test]
    public void CreateTodoItem_MissingIsComplete()
    {
        
    }
    
    
}