using System.Net;
using FluentAssertions;
using Tests.Models;

namespace Tests.TestFixtures;

public class GetOneTodoItemTests
{
    private readonly Helpers _helpers;
    
    public GetOneTodoItemTests()
    {
        _helpers = new Helpers("http://localhost:8080/api");
    }
    
    [Test]
    public void GetOneTodoItemSuccess()
    {
        var id = 1;
        var endpoint = $"TodoItems/{id}";
        var (responseBody, statusCode) = _helpers.ExecuteGetOneRequest<TodoItemModel>(endpoint, id);
        
        statusCode.Should().Be(HttpStatusCode.OK);
        responseBody.Should().NotBeNull();
    }
    
}