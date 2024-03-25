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
        var createdId = Helpers.GenerateId(payload);
        
        // act
        var statusCode = Helpers.ExecuteDeleteRequest(Endpoint, createdId);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.NoContent);
        
        var (_, getTodoItemStatusCode ) = Helpers.ExecuteGetOneRequest<TodoItemResponseModel>(Endpoint, createdId);
        getTodoItemStatusCode.Should().Be(HttpStatusCode.NotFound);
        
    }

    [Test]
    public void DeleteTodoItem_InvalidId()
    {
        // arrange
        
        // act
        var statusCode = Helpers.ExecuteDeleteRequest(Endpoint, "abc123");
        
        // assert
        statusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    public void DeleteTodoItem_NonExistentId()
    {
        // arrange
        
        // act
        var statusCode = Helpers.ExecuteDeleteRequest(Endpoint, 998);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public void DeleteTodoItem_RepeatedDeletion()
    {
        // arrange
        var payload = new TodoItemRequestModel()
        {
            Name = "test repeat deletion, DELETE endpoint",
            IsComplete = true
        };
        var createdId = Helpers.GenerateId(payload);
        
        // act
        var firstDeletionStatusCode = Helpers.ExecuteDeleteRequest(Endpoint, createdId);
        var secondDeletionStatusCode = Helpers.ExecuteDeleteRequest(Endpoint, createdId);

        firstDeletionStatusCode.Should().Be(HttpStatusCode.NoContent);
        secondDeletionStatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    [Ignore("Todo App is still a local implementation and not hosted to a Server")]
    public void DeleteTodoItem_ServerError()
    {
        
    }
}