using System.Net;
using FluentAssertions;
using Tests.Models;

namespace Tests.TestFixtures;

public class DeleteTodoItemTests
{
    private readonly Helpers _helpers;
    
    public DeleteTodoItemTests()
    {
        _helpers = new Helpers("http://localhost:8080/api");
    }
    
    [Test]
    public void DeleteTodoItemSuccess()
    {
        var id = 1;
        var endpoint = "http://localhost:8080/api/TodoItems";
        var statusCode = _helpers.ExecuteDeleteRequest(endpoint, id);
        
        statusCode.Should().Be(HttpStatusCode.NoContent);
    }
}