using System.Net;
using Newtonsoft.Json;
using FluentAssertions;
using Tests.Models;

namespace Tests.TestFixtures;

[TestFixture, Order(1)]
public class CreateTodoItemTests: BaseTestFixture
{
    [Test]
    public void CreateTodoItem_Success_IsCompleteFalse()
    {
        // arrange
        var payload = new TodoItemRequestModel
        {
            Id = 1,
            Name = "test3",
            IsComplete = false
        };
        
        // act
        var (responseBody, statusCode) = Helpers.ExecutePostRequest<TodoItemResponseModel>(Endpoint, payload);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.Created);
        responseBody.Should().NotBeNull();
        responseBody.CreatedTime.Should().NotBe(null);
        responseBody.CompletedTime.Should().Be(null);
        responseBody.Id.Should().NotBe(null);
        responseBody.Name.Should().Be(payload.Name);
    }
    
    [Test]
    public void CreateTodoItem_Success_IsCompleteTrue()
    {
        // arrange
        var payload = new TodoItemRequestModel
        {
            Id = 1,
            Name = "test4",
            IsComplete = true
        };
        
        // act
        var (responseBody, statusCode) = Helpers.ExecutePostRequest<TodoItemResponseModel>(Endpoint, payload);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.Created);
        responseBody.Should().NotBeNull();
        responseBody.CreatedTime.Should().NotBe(null);
        responseBody.CompletedTime.Should().NotBe(null);
        responseBody.Id.Should().NotBe(null);
        responseBody.Name.Should().Be(payload.Name);
    }

    [Test]
    public void CreateTodoItem_MissingId()
    {
        // arrange
        var payload = new TodoItemRequestModel
        {
            Name = "test missing Id",
            IsComplete = false
        };
        
        // act
        var (responseBody, statusCode) = Helpers.ExecutePostRequest<TodoItemResponseModel>(Endpoint, payload);
        
        // arrange
        statusCode.Should().Be(HttpStatusCode.Created);
        responseBody.Should().NotBeNull();
        responseBody.Id.Should().NotBe(null);
        responseBody.CreatedTime.Should().NotBe(null);
        responseBody.CompletedTime.Should().Be(null);
        responseBody.Name.Should().Be(payload.Name);
    }   

    [Test]
    public void CreateTodoItem_MissingName()
    {
        // Name would be null but giving 201. Should be 400.
        // Field validation needs to be implemented.
        // arrange
        var payload = new TodoItemRequestModel()
        {
            IsComplete = false
        };
        
        // act
        var (_, statusCode) = Helpers.ExecutePostRequest<TodoItemResponseModel>(Endpoint, payload);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    public void CreateTodoItem_MissingIsComplete()
    {
        // isComplete would be defaulted to null, returning 201. Should be 400.
        // Field validation needs to be implemented.
        // arrange
        var payload = new TodoItemRequestModel()
        {
            Name = "test missing isComplete field"
        };
        
        // act
        var (_, statusCode) = Helpers.ExecutePostRequest<TodoItemResponseModel>(Endpoint, payload);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    public void CreateTodoItem_AddExtraFieldInPayload()
    {
        // Returning 201 but should be 400.
        // Field validation needs to be implemented. 
        // arrange
        var payload = new Dictionary<string, object>
        {
            { "Id", 1 },
            { "Name", "Todo Item 1" },
            { "IsComplete", false },
            { "FieldNotIncluded", "Some string" }
            
        };
        string addedFieldPayload = JsonConvert.SerializeObject(payload);
        
        // act
        var (_, statusCode) = Helpers.ExecutePostRequest<TodoItemResponseModel>(Endpoint, addedFieldPayload);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    [Test]
    public void CreateTodoItem_EmptyRequestBody()
    {
        // Currently 201 but should be 400. Assigns null in name and false in isComplete.
        // Field validation needs to be implemented.
        // arrange
        var payload = new TodoItemRequestModel{};
        
        // act
        var (_, statusCode) = Helpers.ExecutePostRequest<TodoItemResponseModel>(Endpoint, payload);
        
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
        var (_, statusCode) = Helpers.ExecutePostRequest<TodoItemResponseModel>(Endpoint, modifiedPayload);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    public void CreateTodoItem_NameFieldLimit()
    {
        // No limit currently. Should be limited to something like 256 char.
        // Field validation needs to be implemented.
        // arrange
        var payload = new TodoItemRequestModel()
        {
            Name =
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
            IsComplete = false
        }; // 1310 characters
        
        // act
        var (_, statusCode) = Helpers.ExecutePostRequest<TodoItemResponseModel>(Endpoint, payload);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Test]
    [Ignore("Todo App is still a local implementation and not hosted to a Server")]
    public void CreateTodoItem_ServerErrors()
    {
    }
        
    
}