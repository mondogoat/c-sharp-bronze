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
    
    // TODO create setup where user creates one todo item first, then do a get one call using the same id to validate changes
    [SetUp]
    public void GenerateTodoItem()
    {
        var createRequestPayload = new TodoItemModel()
        {
            name = "test PUT endpoint",
            isComplete = false
        };
        _createdId = _helpers.GenerateId(createRequestPayload);
    }
    
    [Test]
    public void EditTodoItemSuccess()
    {
        // arrange
        var expectedName = "restsharp edit";
        var payload = new TodoItemModel()
        {
            id = _createdId,
            name = "restsharp edit",
            isComplete = true
        };
        
        // act
        var (responseBody, statusCode) = _helpers.ExecutePutRequest<TodoItemModel>(_endpoint, _createdId, payload);
        var x = JsonConvert.SerializeObject(responseBody);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.NoContent);
        
        var (editedTodoItem, _) = _helpers.ExecuteGetOneRequest<TodoItemModel>(_endpoint, _createdId);
        editedTodoItem.name.Should().Be(expectedName);
    }
}