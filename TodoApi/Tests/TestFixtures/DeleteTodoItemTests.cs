using System.Net;
using FluentAssertions;
using Tests.Models;

namespace Tests.TestFixtures;

public class DeleteTodoItemTests : TestFixtureBase
{
    [Test]
    public void DeleteTodoItemSuccess()
    {
        var endpoint = "http://localhost:8080/api/TodoItems";
        var statusCode = ExecuteDeleteRequest(endpoint, 999);
        
        statusCode.Should().Be(HttpStatusCode.NoContent);
    }
}