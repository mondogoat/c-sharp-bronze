using System.Net;
using Newtonsoft.Json;
using RestSharp;
using FluentAssertions;
using NUnit.Framework.Internal;
using Tests.Models;

namespace Tests.TestFixtures;

[TestFixture, Order(5)]
public class GetAllTodoTests
{
    private readonly Helpers _helpers;
    private readonly string _endpoint = "TodoItems";
    public GetAllTodoTests()
    {
        _helpers = new Helpers("http://localhost:8080/api");
    }
    
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
            
            _helpers.ExecutePostRequest<TodoItemResponseModel>(_endpoint, payload);
        }
    }

    [TearDown]
    public void ClearTodoList()
    {
        var (todoItems, _) = _helpers.ExecuteGetRequest<List<TodoItemResponseModel>>(_endpoint);

        if (todoItems.Any())
        {
            foreach (var todoItem in todoItems)
            {
                _helpers.ExecuteDeleteRequest(_endpoint, todoItem.Id);
            }
        }
    }
    
    [Test]
    public void GetAllTodo_Success()
    {
        // arrange
        
        // act
        var (responseBody, statusCode) = _helpers.ExecuteGetRequest<List<TodoItemResponseModel>>(_endpoint);
        Console.WriteLine(JsonConvert.SerializeObject(responseBody));
        
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
        var (responseBody, statusCode) = _helpers.ExecuteGetRequest<List<TodoItemResponseModel>>(_endpoint);
        
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