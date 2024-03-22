using System.Net;
using FluentAssertions;
using Tests.Models;

namespace Tests.TestFixtures;

[TestFixture, Order(4)]
public class DeleteTodoItemTests : BaseTestFixture
{
    
    [Test]
    public void DeleteTodoItem_Success()
    {
        // arrange
        var payload = new TodoItemRequestModel()
        {
            Name = "test DELETE endpoint",
            IsComplete = false
        };
        var createdId = _helpers.GenerateId(payload);
        
        // act
        var statusCode = _helpers.ExecuteDeleteRequest(Endpoint, createdId);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.NoContent);
        
        var (_, getTodoItemStatusCode ) = _helpers.ExecuteGetOneRequest<TodoItemResponseModel>(Endpoint, createdId);
        getTodoItemStatusCode.Should().Be(HttpStatusCode.NotFound);
        
    }

    [Test]
    public void DeleteTodoItem_InvalidId()
    {
        // arrange
        
        var statusCode = _helpers.ExecuteDeleteRequest(Endpoint, "abc123");
        
        // assert
        statusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    public void DeleteTodoItem_NonExistentId()
    {
        var statusCode = _helpers.ExecuteDeleteRequest(Endpoint, 998);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.NotFound);
    }
}