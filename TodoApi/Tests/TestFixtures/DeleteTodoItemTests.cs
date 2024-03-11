using System.Net;
using FluentAssertions;
using Tests.Models;

namespace Tests.TestFixtures;

[TestFixture, Order(4)]
public class DeleteTodoItemTests
{
    private readonly Helpers _helpers;
    private readonly string _endpoint = "TodoItems";
    
    public DeleteTodoItemTests()
    {
        _helpers = new Helpers("http://localhost:8080/api");
    }
    
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
        var statusCode = _helpers.ExecuteDeleteRequest(_endpoint, createdId);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.NoContent);
        
        var (_, getTodoItemStatusCode ) = _helpers.ExecuteGetOneRequest<TodoItemResponseModel>(_endpoint, createdId);
        getTodoItemStatusCode.Should().Be(HttpStatusCode.NotFound);
        
    }

    [Test]
    public void DeleteTodoItem_InvalidId()
    {
        // arrange
        
        var statusCode = _helpers.ExecuteDeleteRequest(_endpoint, "abc123");
        
        // assert
        statusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    public void DeleteTodoItem_NonExistentId()
    {
        var statusCode = _helpers.ExecuteDeleteRequest(_endpoint, 998);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.NotFound);
    }
}