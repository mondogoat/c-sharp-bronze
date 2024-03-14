using System.Net;
using FluentAssertions;
using Tests.Models;

namespace Tests.TestFixtures;

[TestFixture, Order(3)]
public class EditTodoItemTests : BaseTestFixture
{
    private int _createdId;
    
    [SetUp]
    public void GenerateTodoItem()
    {
        var createRequestPayload = new TodoItemRequestModel()
        {
            Name = "test PUT endpoint",
            IsComplete = false
        };
        _createdId = _helpers.GenerateId(createRequestPayload);
        Console.WriteLine(_createdId);
    }
    
    [Test]
    public void EditTodoItem_SuccessSetIsCompleteFromFalseToTrue()
    {
        // arrange
        var expectedName = "restsharp edit";
        var payload = new TodoItemRequestModel()
        {
            Id = _createdId,
            Name = "restsharp edit",
            IsComplete = true
        };
        
        // act
        var (_, statusCode) = _helpers.ExecutePutRequest<TodoItemResponseModel>(Endpoint, _createdId, payload);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.NoContent);
        
        var (editedTodoItem, _) = _helpers.ExecuteGetOneRequest<TodoItemResponseModel>(Endpoint, _createdId);
        editedTodoItem.Name.Should().Be(expectedName);
        editedTodoItem.CompletedTime.Should().NotBe(null);
    }
    
    [Test]
    public void EditTodoItem_SuccessSetIsCompleteFromTrueToFalse()
    {
        // arrange
        var expectedName = "restsharp edit";
        var payload1 = new TodoItemRequestModel()
        {
            Id = _createdId,
            Name = "restsharp edit",
            IsComplete = true
        };
        
        // act
        // Set isComplete of createdId from false to true
        var (_, statusCode1) = _helpers.ExecutePutRequest<TodoItemResponseModel>(Endpoint, _createdId, payload1);
        
        // assert
        statusCode1.Should().Be(HttpStatusCode.NoContent);
        
        var (editedTodoItem1, _) = _helpers.ExecuteGetOneRequest<TodoItemResponseModel>(Endpoint, _createdId);
        editedTodoItem1.Name.Should().Be(expectedName);
        editedTodoItem1.CompletedTime.Should().NotBe(null);
        
        // Then set it back to false
        // arrange
        var payload2 = new TodoItemRequestModel()
        {
            Id = _createdId,
            Name = "restsharp edit",
            IsComplete = false
        };
        
        // act
        var (_, statusCode2) = _helpers.ExecutePutRequest<TodoItemResponseModel>(Endpoint, _createdId, payload2);
        
        // assert
        statusCode2.Should().Be(HttpStatusCode.NoContent);
        
        var (editedTodoItem, _) = _helpers.ExecuteGetOneRequest<TodoItemResponseModel>(Endpoint, _createdId);
        editedTodoItem.Name.Should().Be(expectedName);
        editedTodoItem.CompletedTime.Should().Be(null);
    }

    [Test]
    public void EditTodoItem_InvalidId()
    {
        // arrange
        var payload = new TodoItemRequestModel()
        {
            Id = _createdId,
            Name = "restsharp edit",
            IsComplete = true
        };
        
        // act
        var (_, statusCode) = _helpers.ExecutePutRequest<TodoItemResponseModel>(Endpoint, "abc", payload);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var (editedTodoItem, _) = _helpers.ExecuteGetOneRequest<TodoItemResponseModel>(Endpoint, _createdId);
        editedTodoItem.Name.Should().Be("test PUT endpoint");
        
    }

    [Test]
    public void EditTodoItem_NonExistentId()
    {
        // arrange
        var payload = new TodoItemRequestModel()
        {
            Id = 999,
            Name = "restsharp edit",
            IsComplete = true
        };
        
        // act
        var (_, statusCode) = _helpers.ExecutePutRequest<TodoItemResponseModel>(Endpoint, 999, payload);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Test]
    public void EditTodoItem_MissingName()
    {
        // current behavior: sets name to null. should instead throw 400.
        var payload = new TodoItemRequestModel()
        {
            Id = _createdId,
            IsComplete = true
        };
        
        // act
        var (_, statusCode) = _helpers.ExecutePutRequest<TodoItemResponseModel>(Endpoint, _createdId, payload);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var (editedTodoItem, _) = _helpers.ExecuteGetOneRequest<TodoItemResponseModel>(Endpoint, _createdId);
        editedTodoItem.Name.Should().Be("test PUT endpoint");
    }
    
    [Test]
    public void EditTodoItem_MissingIsComplete()
    {
        // defaults to false after success if isComplete is not passed in request body. should be 400.
        var payload = new TodoItemRequestModel()
        {
            Id = _createdId,
            Name = "todo missing isComplete"
        };
        
        // act
        var (_, statusCode) = _helpers.ExecutePutRequest<TodoItemResponseModel>(Endpoint, _createdId, payload);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var (editedTodoItem, _) = _helpers.ExecuteGetOneRequest<TodoItemResponseModel>(Endpoint, _createdId);
        editedTodoItem.IsComplete.Should().Be(false);
        editedTodoItem.CompletedTime.Should().Be(null);
    }
    
    [Test]
    public void EditTodoItem_MissingIdInRequestBody()
    {
        // should be 204. id should not be required since we are already passing id in query param. current behavior is 400. 
        var payload = new TodoItemRequestModel()
        {
            Name = "todo missing ID",
            IsComplete = true
        };
        
        // act
        var (_, statusCode) = _helpers.ExecutePutRequest<TodoItemResponseModel>(Endpoint, _createdId, payload);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.NoContent);
        
        var (editedTodoItem, _) = _helpers.ExecuteGetOneRequest<TodoItemResponseModel>(Endpoint, _createdId);
        editedTodoItem.Name.Should().Be("todo missing ID");
        editedTodoItem.IsComplete.Should().Be(true);
    }
    
    [Test]
    public void EditTodoItem_EmptyRequestBody()
    {
        var payload = new TodoItemRequestModel{};
        
        // act
        var (_, statusCode) = _helpers.ExecutePutRequest<TodoItemResponseModel>(Endpoint, _createdId, payload);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var (editedTodoItem, _) = _helpers.ExecuteGetOneRequest<TodoItemResponseModel>(Endpoint, _createdId);
        editedTodoItem.Name.Should().Be("test PUT endpoint");
        editedTodoItem.IsComplete.Should().Be(false);
    }
    
    [Test]
    public void EditTodoItem_InvalidDataType()
    {
        var payload = new Dictionary<string, object>
        {
            {"id", _createdId},
            { "name", 8888 },
            { "isComplete", "some string" }
        };
        
        // act
        var (_, statusCode) = _helpers.ExecutePutRequest<TodoItemResponseModel>(Endpoint, _createdId, payload);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var (editedTodoItem, _) = _helpers.ExecuteGetOneRequest<TodoItemResponseModel>(Endpoint, _createdId);
        editedTodoItem.Name.Should().Be("test PUT endpoint");
        editedTodoItem.IsComplete.Should().Be(false);
    }
    
}