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
            bool isComplete = i % 2 == 0; // alternate isComplete value, false first
            var payload = new TodoItemRequestModel()
            {
                Name = $"todo item {i}",
                IsComplete = isComplete
            };
            
            Helpers.ExecutePostRequest<TodoItemResponseModel>(Endpoint, payload);
        }
    }
    
    [Test]
    public void GetAllTodo_Success()
    {
        // arrange
        
        // act
        var (responseBody, statusCode) = Helpers.ExecuteGetRequest<List<TodoItemResponseModel>>(Endpoint);
        
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
        var (responseBody, statusCode) = Helpers.ExecuteGetRequest<List<TodoItemResponseModel>>(Endpoint);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.OK);
        responseBody.Should().BeEmpty();
    }
    
    [Test]
    public void GetAllTodo_IsCompleteValueFilterFalse()
    {
        // arrange
        
        // act
        var (responseBody, statusCode) = Helpers.ExecuteGetRequest<List<TodoItemResponseModel>>("TodoItems?isComplete=false");
        Console.WriteLine(JsonConvert.SerializeObject(responseBody));
       
        // assert
        statusCode.Should().Be(HttpStatusCode.OK);
        foreach (var item in responseBody)
        {
            item.IsComplete.Should().Be(false);
            item.CompletedTime.Should().Be(null);
        }
    }

    [Test]
    public void GetAllTodo_IsCompleteValueFilterTrue()
    {
        // arrange
        
        // act
        var (responseBody, statusCode) = Helpers.ExecuteGetRequest<List<TodoItemResponseModel>>("TodoItems?isComplete=true");
        Console.WriteLine(JsonConvert.SerializeObject(responseBody));
        
        // assert
        statusCode.Should().Be(HttpStatusCode.OK);
        
        foreach (var item in responseBody)
        {
            item.IsComplete.Should().Be(true);
            item.CompletedTime.Should().NotBe(null);
        }
    }
    
    [Test]
    [Ignore("Pagination not yet implemented")]
    public void GetAllTodo_Pagination()
    {
    }
}