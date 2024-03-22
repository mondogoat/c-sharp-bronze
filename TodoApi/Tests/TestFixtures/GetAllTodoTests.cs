using System.Net;
using FluentAssertions;
using Newtonsoft.Json;
using Tests.Models;

namespace Tests.TestFixtures;

[TestFixture, Order(5)]
public class GetAllTodoTests: BaseTestFixture
{
    [SetUp]
    public void PopulateTodoList()
    {
        for (int i = 1; i <= 5; i++)
        {
            // alternate isComplete value, false first
            bool isComplete = i % 2 == 0; 
            var payload = new TodoItemRequestModel()
            {
                Name = $"todo item {i}",
                IsComplete = isComplete
            };
            
            _helpers.ExecutePostRequest<TodoItemResponseModel>(Endpoint, payload);
        }
    }

    [TearDown]
    public void ClearTodoList()
    {
        var (todoItems, _) = _helpers.ExecuteGetRequest<List<TodoItemResponseModel>>(Endpoint);
    
        if (todoItems.Any())
        {
            foreach (var todoItem in todoItems)
            {
                _helpers.ExecuteDeleteRequest(Endpoint, todoItem.Id);
            }
        }
    }
    
    [Test]
    public void GetAllTodo_Success()
    {
        // arrange
        
        // act
        var (responseBody, statusCode) = _helpers.ExecuteGetRequest<List<TodoItemResponseModel>>(Endpoint);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.OK);
        responseBody.Should().NotBeNullOrEmpty();
    }
    
    [Test]
    public void GetAllTodo_SuccessWithEmptyListIfNoTodoItem()
    {
        // arrange
        ClearTodoList();
        
        // act
        var (responseBody, statusCode) = _helpers.ExecuteGetRequest<List<TodoItemResponseModel>>(Endpoint);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.OK);
        Console.WriteLine(JsonConvert.SerializeObject(responseBody));
        responseBody.Should().BeEmpty();
    }

    [Test]
    public void GetAllTodo_IsCompleteValueFilterTrue()
    {
        // arrange
        
        // act
        var (responseBody, statusCode) = _helpers.ExecuteGetRequest<List<TodoItemResponseModel>>("TodoItems?isComplete=true");
        Console.WriteLine(JsonConvert.SerializeObject(responseBody));
        
        // assert
        statusCode.Should().Be(HttpStatusCode.OK);
        responseBody.Count.Should().Be(2);
    }
    
    [Test]
    public void GetAllTodo_IsCompleteValueFilterFalse()
    {
        // arrange
        
        // act
        var (responseBody, statusCode) = _helpers.ExecuteGetRequest<List<TodoItemResponseModel>>("TodoItems?isComplete=false");
       
        // assert
        statusCode.Should().Be(HttpStatusCode.OK);
        responseBody.Count.Should().Be(3);
    }
    
    [Test]
    [Ignore("Pagination not yet implemented")]
    public void GetAllTodo_Pagination()
    {
    }
}