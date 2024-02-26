using System.Net;
using FluentAssertions;
using Tests.Models;

namespace Tests.TestFixtures;

public class EditTodoItemTests : TestFixtureBase
{
    // TODO create setup where user creates one todo item first, then do a get one call using the same id to validate changes
    [Test]
    public void EditTodoItemSuccess()
    {
        var endpoint = "http://localhost:8080/api/TodoItems";
        var (responseBody, statusCode) = ExecutePutRequest<TodoItemModel>(endpoint, 999);
        
        statusCode.Should().Be(HttpStatusCode.NoContent);
    }
}