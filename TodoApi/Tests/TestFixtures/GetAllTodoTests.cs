using System.Net;
using Newtonsoft.Json;
using RestSharp;
using FluentAssertions;
using NUnit.Framework.Internal;
using Tests.Models;

namespace Tests.TestFixtures;

[TestFixture]
public class GetAllTodoTests
{
    private readonly TestUtilities _testUtilities;
    private readonly string _endpoint = "TodoItems";
    public GetAllTodoTests()
    {
        _testUtilities = new TestUtilities("http://localhost:8080/api");
    }
    
    [SetUp]
    public void PopulateTodoList()
    {
        for (int i = 1; i <= 5; i++)
        {
            var payload = new TodoItemModel
            {
                name = $"todo item {i}",
                isComplete = false
            };
            
            _testUtilities.ExecutePostRequest<TodoItemModel>(_endpoint, payload);
        }
    }

    [TearDown]
    public void ClearTodoList()
    {
        var (todoItems, _) = _testUtilities.ExecuteGetRequest<List<TodoItemModel>>(_endpoint);

        if (todoItems != null && todoItems.Any())
        {
            foreach (var todoItem in todoItems)
            {
                _testUtilities.ExecuteDeleteRequest(_endpoint, todoItem.id);
            }
        }
    }
    
    [Test]
    public void GetAllTodo_Success()
    {
        // TODO: Create 5 TodoItems first then execute GET request to fetch list of TODO
        // arrange
        
        // act
        var (responseBody, statusCode) = _testUtilities.ExecuteGetRequest<List<TodoItemModel>>(_endpoint);
        Console.WriteLine(JsonConvert.SerializeObject(responseBody));
        
        // assert
        statusCode.Should().Be(HttpStatusCode.OK);
        responseBody.Should().NotBeNullOrEmpty();
    }

    [Test]
    public void GetAllTodo_IsCompleteValueFilterTrue()
    {
    }
    
    [Test]
    public void GetAllTodo_IsCompleteValueFilterFalse()
    {
    }

    [Test]
    public void GetAllTodo_SuccessWithEmptyListIfNoTodoItem()
    {
        // arrange
        ClearTodoList();
        
        // act
        var (responseBody, statusCode) = _testUtilities.ExecuteGetRequest<List<TodoItemModel>>(_endpoint);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.OK);
        responseBody.Should().BeEmpty();
    }

    [Test]
    public void GetAllTodo_Pagination()
    {
    }
}