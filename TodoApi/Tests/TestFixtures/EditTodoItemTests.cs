using System.Net;
using FluentAssertions;
using Tests.Models;

namespace Tests.TestFixtures;

[TestFixture, Order(3)]
public class EditTodoItemTests : BaseTestFixture
{
    private int _createdIdFalse;
    private int _createdIdTrue;
    
    [SetUp]
    public void GenerateTodoItems()
    {
        // First TodoItem with IsComplete=false
        var createRequestPayload1 = new TodoItemRequestModel()
        {
            Name = "test PUT endpoint - isComplete=false",
            IsComplete = false
        };
        _createdIdFalse = Helpers.GenerateId(createRequestPayload1);

        // Second TodoItem with IsComplete=true
        var createRequestPayload2 = new TodoItemRequestModel()
        {
            Name = "test PUT endpoint - isComplete=true",
            IsComplete = true
        };
        _createdIdTrue = Helpers.GenerateId(createRequestPayload2);
    }
    
    [Test]
    public void EditTodoItem_SuccessSetIsCompleteFromFalseToTrue()
    {
        // arrange
        var expectedName = "restsharp edit";
        var payload = new TodoItemRequestModel()
        {
            Id = _createdIdFalse,
            Name = "restsharp edit",
            IsComplete = true
        };
        
        // act
        var (_, statusCode) = Helpers.ExecutePutRequest<TodoItemResponseModel>(Endpoint, _createdIdFalse, payload);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.NoContent);
        
        var (editedTodoItem, _) = Helpers.ExecuteGetOneRequest<TodoItemResponseModel>(Endpoint, _createdIdFalse);
        editedTodoItem.Name.Should().Be(expectedName);
        editedTodoItem.CompletedTime.Should().NotBe(null);
    }
    
    [Test]
    public void EditTodoItem_SuccessSetIsCompleteFromTrueToFalse()
    {
        // arrange
        var expectedName = "restsharp edit";
        var payload = new TodoItemRequestModel()
        {
            Id = _createdIdTrue,
            Name = "restsharp edit",
            IsComplete = false
        };
        
        // act
        // Set isComplete of createdId from false to true
        var (_, statusCode) = Helpers.ExecutePutRequest<TodoItemResponseModel>(Endpoint, _createdIdTrue, payload);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.NoContent);
        
        var (editedTodoItem, _) = Helpers.ExecuteGetOneRequest<TodoItemResponseModel>(Endpoint, _createdIdTrue);
        editedTodoItem.Name.Should().Be(expectedName);
        editedTodoItem.CompletedTime.Should().Be(null);
    }

    [Test]
    public void EditTodoItem_InvalidId()
    {
        // arrange
        var payload = new TodoItemRequestModel()
        {
            Id = _createdIdFalse,
            Name = "restsharp edit",
            IsComplete = true
        };
        
        // act
        var (_, statusCode) = Helpers.ExecutePutRequest<TodoItemResponseModel>(Endpoint, "abc", payload);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var (editedTodoItem, _) = Helpers.ExecuteGetOneRequest<TodoItemResponseModel>(Endpoint, _createdIdFalse);
        editedTodoItem.Name.Should().Be("test PUT endpoint - isComplete=false");
        
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
        var (_, statusCode) = Helpers.ExecutePutRequest<TodoItemResponseModel>(Endpoint, 999, payload);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Test]
    public void EditTodoItem_MissingName()
    {
        // current behavior: sets name to null. should instead throw 400.
        var payload = new TodoItemRequestModel()
        {
            Id = _createdIdFalse,
            IsComplete = true
        };
        
        // act
        var (_, statusCode) = Helpers.ExecutePutRequest<TodoItemResponseModel>(Endpoint, _createdIdFalse, payload);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var (editedTodoItem, _) = Helpers.ExecuteGetOneRequest<TodoItemResponseModel>(Endpoint, _createdIdFalse);
        editedTodoItem.Name.Should().Be("test PUT endpoint - isComplete=false");
    }
    
    [Test]
    public void EditTodoItem_MissingIsComplete()
    {
        // defaults to false after success if isComplete is not passed in request body. should be 400.
        var payload = new TodoItemRequestModel()
        {
            Id = _createdIdFalse,
            Name = "todo missing isComplete"
        };
        
        // act
        var (_, statusCode) = Helpers.ExecutePutRequest<TodoItemResponseModel>(Endpoint, _createdIdFalse, payload);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var (editedTodoItem, _) = Helpers.ExecuteGetOneRequest<TodoItemResponseModel>(Endpoint, _createdIdFalse);
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
        var (_, statusCode) = Helpers.ExecutePutRequest<TodoItemResponseModel>(Endpoint, _createdIdFalse, payload);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.NoContent);
        
        var (editedTodoItem, _) = Helpers.ExecuteGetOneRequest<TodoItemResponseModel>(Endpoint, _createdIdFalse);
        editedTodoItem.Name.Should().Be("todo missing ID");
        editedTodoItem.IsComplete.Should().Be(true);
    }
    
    [Test]
    public void EditTodoItem_EmptyRequestBody()
    {
        var payload = new TodoItemRequestModel{};
        
        // act
        var (_, statusCode) = Helpers.ExecutePutRequest<TodoItemResponseModel>(Endpoint, _createdIdFalse, payload);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var (editedTodoItem, _) = Helpers.ExecuteGetOneRequest<TodoItemResponseModel>(Endpoint, _createdIdFalse);
        editedTodoItem.Name.Should().Be("test PUT endpoint - isComplete=false");
        editedTodoItem.IsComplete.Should().Be(false);
    }
    
    [Test]
    public void EditTodoItem_InvalidDataType()
    {
        var payload = new Dictionary<string, object>
        {
            {"id", _createdIdFalse},
            { "name", 8888 },
            { "isComplete", "some string" }
        };
        
        // act
        var (_, statusCode) = Helpers.ExecutePutRequest<TodoItemResponseModel>(Endpoint, _createdIdFalse, payload);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var (editedTodoItem, _) = Helpers.ExecuteGetOneRequest<TodoItemResponseModel>(Endpoint, _createdIdFalse);
        editedTodoItem.Name.Should().Be("test PUT endpoint - isComplete=false");
        editedTodoItem.IsComplete.Should().Be(false);
    }

    [Test]
    [Ignore("Logic on change of isComplete value from True to True is not yet implemented")]
    public void EditTodoItem_EditIsCompleteFromTrueToTrue()
    {
        
    }
    
}