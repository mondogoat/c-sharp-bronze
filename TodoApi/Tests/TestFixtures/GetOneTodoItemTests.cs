using System.Net;
using FluentAssertions;
using Tests.Models;

namespace Tests.TestFixtures;

public class GetOneTodoItemTests: TestFixtureBase
{
    [Test]
    public void GetOneTodoItemSuccess()
    {
        var endpoint = "http://localhost:8080/api/TodoItems";
        var (responseBody, statusCode) = ExecuteGetOneRequest<TodoItemModel>(endpoint, 1);
        
        statusCode.Should().Be(HttpStatusCode.OK);
        responseBody.Should().NotBeNull();
    }
    
}