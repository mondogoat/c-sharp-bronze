using System.Net;
using FluentAssertions;
using Newtonsoft.Json;
using Tests.Models;

namespace Tests.TestFixtures;

public class EditTodoItemTests
{
    private readonly Helpers _helpers;
    private readonly string _endpoint = "TodoItems";
    private int _createdId;
    

    public EditTodoItemTests()
    {
        _helpers = new Helpers("http://localhost:8080/api");
    }
    
    [SetUp]
    public void GenerateTodoItem()
    {
        var createRequestPayload = new TodoItemModel()
        {
            Name = "test PUT endpoint",
            IsComplete = false
        };
        _createdId = _helpers.GenerateId(createRequestPayload);
        Console.WriteLine(_createdId);
    }
    
    [Test]
    public void EditTodoItem_Success()
    {
        // arrange
        var expectedName = "restsharp edit";
        var payload = new TodoItemModel()
        {
            Id = _createdId,
            Name = "restsharp edit",
            IsComplete = true
        };
        
        // act
        var (responseBody, statusCode) = _helpers.ExecutePutRequest<TodoItemModel>(_endpoint, _createdId, payload);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.NoContent);
        
        var (editedTodoItem, _) = _helpers.ExecuteGetOneRequest<TodoItemModel>(_endpoint, _createdId);
        editedTodoItem.Name.Should().Be(expectedName);
    }

    [Test]
    public void EditTodoItem_InvalidId()
    {
        // arrange
        var payload = new TodoItemModel()
        {
            Id = _createdId,
            Name = "restsharp edit",
            IsComplete = true
        };
        
        // act
        var (responseBody, statusCode) = _helpers.ExecutePutRequest<TodoItemModel>(_endpoint, "abc", payload);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var (editedTodoItem, _) = _helpers.ExecuteGetOneRequest<TodoItemModel>(_endpoint, _createdId);
        editedTodoItem.Name.Should().Be("test PUT endpoint");
        
    }

    [Test]
    public void EditTodoItem_NonExistentId()
    {
        // fails because the id in response body does not match with the id passed in the param.
        // should have id not required and only follow what is passed in the query param.
        // arrange
        var payload = new TodoItemModel()
        {
            Id = _createdId,
            Name = "restsharp edit",
            IsComplete = true
        };
        
        // act
        var (responseBody, statusCode) = _helpers.ExecutePutRequest<TodoItemModel>(_endpoint, 999, payload);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Test]
    public void EditTodoItem_MissingName()
    {
        // current behavior: sets name to null. should instead throw 400.
        var payload = new TodoItemModel()
        {
            Id = _createdId,
            IsComplete = true
        };
        
        // act
        var (responseBody, statusCode) = _helpers.ExecutePutRequest<TodoItemModel>(_endpoint, _createdId, payload);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var (editedTodoItem, _) = _helpers.ExecuteGetOneRequest<TodoItemModel>(_endpoint, _createdId);
        editedTodoItem.Name.Should().Be("test PUT endpoint");
        Console.WriteLine(JsonConvert.SerializeObject(editedTodoItem));
    }
    
    [Test]
    public void EditTodoItem_MissingIsComplete()
    {
        // defaults to false after success. should be 400.
        var payload = new TodoItemModel()
        {
            Id = _createdId,
            Name = "todo missing isComplete"
        };
        
        // act
        var (responseBody, statusCode) = _helpers.ExecutePutRequest<TodoItemModel>(_endpoint, _createdId, payload);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var (editedTodoItem, _) = _helpers.ExecuteGetOneRequest<TodoItemModel>(_endpoint, _createdId);
        editedTodoItem.IsComplete.Should().Be(false);
    }
    
    [Test]
    public void EditTodoItem_MissingIdInRequestBody()
    {
        // should be 204. id should not be required since we are already passing id in query param. current behavior is 400. 
        var payload = new TodoItemModel()
        {
            Name = "todo missing ID",
            IsComplete = true
        };
        
        // act
        var (responseBody, statusCode) = _helpers.ExecutePutRequest<TodoItemModel>(_endpoint, _createdId, payload);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.NoContent);
        
        var (editedTodoItem, _) = _helpers.ExecuteGetOneRequest<TodoItemModel>(_endpoint, _createdId);
        editedTodoItem.Name.Should().Be("todo missing ID");
        editedTodoItem.IsComplete.Should().Be(true);
    }
    
    [Test]
    public void EditTodoItem_EmptyRequestBody()
    {
        var payload = new TodoItemModel()
        {};
        
        // act
        var (responseBody, statusCode) = _helpers.ExecutePutRequest<TodoItemModel>(_endpoint, _createdId, payload);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var (editedTodoItem, _) = _helpers.ExecuteGetOneRequest<TodoItemModel>(_endpoint, _createdId);
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
        var (responseBody, statusCode) = _helpers.ExecutePutRequest<TodoItemModel>(_endpoint, _createdId, payload);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.BadRequest);
        
        var (editedTodoItem, _) = _helpers.ExecuteGetOneRequest<TodoItemModel>(_endpoint, _createdId);
        editedTodoItem.Name.Should().Be("test PUT endpoint");
        editedTodoItem.IsComplete.Should().Be(false);
    }
    
}