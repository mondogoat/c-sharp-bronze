using System.Net;
using FluentAssertions;
using Tests.Models;

namespace Tests.TestFixtures;

public class EditTodoItemTests
{
    private readonly TestUtilities _testUtilities;

    public EditTodoItemTests()
    {
        _testUtilities = new TestUtilities("http://localhost:8080/api");
    }
    
    // TODO create setup where user creates one todo item first, then do a get one call using the same id to validate changes
    [Test]
    public void EditTodoItemSuccess()
    {
        var id = 1; // TODO call ExecutePostRequest first and get the id and pass it to the payload
        var payload = new TodoItemModel()
        {
            id = 999,
            name = "test3",
            isComplete = false
        };
        var endpoint = "TodoItems/{id}";
        
        var (responseBody, statusCode) = _testUtilities.ExecutePutRequest<TodoItemModel>(endpoint, id, payload);
        
        statusCode.Should().Be(HttpStatusCode.NoContent);
    }
}