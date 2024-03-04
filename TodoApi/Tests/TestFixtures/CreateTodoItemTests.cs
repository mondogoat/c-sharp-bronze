using System.Net;
using Newtonsoft.Json;
using RestSharp;
using FluentAssertions;
using Tests.Models;

namespace Tests.TestFixtures;

public class CreateTodoItemTests
{
    private readonly Helpers _helpers;
    private readonly string _endpoint = "TodoItems";

    public CreateTodoItemTests()
    {
        _helpers = new Helpers("http://localhost:8080/api");
    }

    [Test]
    public void CreateTodoItem_Success()
    {
        // arrange
        var payload = new TodoItemModel
        {
            name = "test3",
            isComplete = false
        };
        
        // act
        var (responseBody, statusCode) = _helpers.ExecutePostRequest<TodoItemModel>(_endpoint, payload);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.Created);
        responseBody.Should().NotBeNull();
        responseBody.id.Should().NotBe(null);
    }

    [Test]
    public void CreateTodoItem_MissingId()
    {
        // arrange
        var payload = new TodoItemModel
        {
            name = "test missing Id",
            isComplete = false
        };
        
        // act
        var (responseBody, statusCode) = _helpers.ExecutePostRequest<TodoItemModel>(_endpoint, payload);
        
        // arrange
        statusCode.Should().Be(HttpStatusCode.Created);
        responseBody.Should().NotBeNull();
        responseBody.id.Should().NotBe(null);
    }

    [Test]
    public void CreateTodoItem_MissingName()
    {
        // name would be null but 201. should be 400.
        // arrange
        var payload = new TodoItemModel()
        {
            isComplete = false
        };
        
        // act
        var (responseBody, statusCode) = _helpers.ExecutePostRequest<TodoItemModel>(_endpoint, payload);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    public void CreateTodoItem_MissingIsComplete()
    {
        // should be 400 error. currently 201 and defaults to false
        // arrange
        var payload = new TodoItemModel()
        {
            name = "test missing isComplete field"
        };
        
        // act
        var (responseBody, statusCode) = _helpers.ExecutePostRequest<TodoItemModel>(_endpoint, payload);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    public void CreateTodoItem_AddExtraFieldInPayload()
    {
        // arrange
        var payload = new Dictionary<string, object>
        {
            { "Id", 1 },
            { "Name", "Todo Item 1" },
            { "IsComplete", false },
            { "dateCreated", DateTime.Now }
            
        };
        string addedFieldPayload = JsonConvert.SerializeObject(payload);
        
        // act
        var (responseBody, statusCode) = _helpers.ExecutePostRequest<TodoItemModel>(_endpoint, addedFieldPayload);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Test]
    public void CreateTodoItem_EmptyRequestBody()
    {
        // arrange
        var payload = new TodoItemModel{ };
        
        // act
        var (responseBody, statusCode) = _helpers.ExecutePostRequest<TodoItemModel>(_endpoint, payload);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Test]
    public void CreateTodoItem_InvalidDataTypes()
    {
        // arrange
        var payload = new Dictionary<string, object>
        {
            { "Name", 8888 },
            { "IsComplete", "some string" }
        };
        string modifiedPayload = JsonConvert.SerializeObject(payload);
        
        // act
        var (responseBody, statusCode) = _helpers.ExecutePostRequest<TodoItemModel>(_endpoint, modifiedPayload);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    public void CreateTodoItem_NameFieldLimit()
    {
        // no limit currently. should be limited to something like 256 char.
        // arrange
        var payload = new TodoItemModel()
        {
            name =
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. " +
                "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in " +
                "reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa " +
                "qui officia deserunt mollit anim id est laborum. Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, " +
                "totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem " +
                "quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam " +
                "est, qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt ut labore et dolore magnam " +
                "aliquam quaerat voluptatem. Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi " +
                "consequatur? Quis autem vel eum iure reprehenderit qui in ea voluptate velit esse quam nihil molestiae consequatur, vel illum " +
                "qui dolorem eum fugiat quo voluptas nulla pariatur?",
            isComplete = false
        }; // 1310 characters
        
        // act
        var (responseBody, statusCode) = _helpers.ExecutePostRequest<TodoItemModel>(_endpoint, payload);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.BadRequest);

    }
        
    
}