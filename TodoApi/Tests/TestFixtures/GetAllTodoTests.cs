using System.Net;
using FluentAssertions;
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
            var payload = new TodoItemRequestModel()
            {
                Name = $"todo item {i}",
                IsComplete = false
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
        responseBody.Should().BeEmpty();
    }

    // [Test]
    // public void GetAllTodo_IsCompleteValueFilterTrue()
    // {
    // }
    //
    // [Test]
    // public void GetAllTodo_IsCompleteValueFilterFalse()
    // {
    // }
    //
    // [Test]
    // public void GetAllTodo_Pagination()
    // {
    // }
}