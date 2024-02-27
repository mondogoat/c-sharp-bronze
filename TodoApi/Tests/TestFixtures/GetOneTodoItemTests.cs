using System.Net;
using FluentAssertions;
using Tests.Models;

namespace Tests.TestFixtures;

public class GetOneTodoItemTests
{
    private readonly TestUtilities _testUtilities;
    
    public GetOneTodoItemTests()
    {
        _testUtilities = new TestUtilities("http://localhost:8080/api");
    }
    
    [Test]
    public void GetOneTodoItemSuccess()
    {
        var id = 1;
        var endpoint = $"TodoItems/{id}";
        var (responseBody, statusCode) = _testUtilities.ExecuteGetOneRequest<TodoItemModel>(endpoint, id);
        
        statusCode.Should().Be(HttpStatusCode.OK);
        responseBody.Should().NotBeNull();
    }
    
}