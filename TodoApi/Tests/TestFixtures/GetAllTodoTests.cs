using System.Net;
using Newtonsoft.Json;
using RestSharp;
using FluentAssertions;
using NUnit.Framework.Internal;
using Tests.Models;

namespace Tests.TestFixtures;

public class GetAllTodoTests
{
    private readonly TestUtilities _testUtilities;

    public GetAllTodoTests()
    {
        _testUtilities = new TestUtilities("http://localhost:8080/api");
    }

    [OneTimeSetUp]
    public void PopulateTodoList()
    {
        var endpoint = "TodoItems";
        
        for (int i = 1; i <= 3; i++)
        {
            var payload = new TodoItemModel
            {
                name = $"todo item {i}",
                isComplete = false
            };
            
            _testUtilities.ExecutePostRequest<TodoItemModel>(endpoint, payload);
        }
    }

    [Test]
    public void GetAllTodoSuccess()
    {
        // TODO: Create 3 TodoItems first then execute GET request to fetch list of TODO
        // arrange
        var endpoint = "TodoItems";
        // var payload = new TodoItemModel
        // {
        //     name = "test3",
        //     isComplete = false
        // };
        // _testUtilities.ExecutePostRequest<TodoItemModel>("TodoItems", payload);
        // _testUtilities.ExecutePostRequest<TodoItemModel>("TodoItems", payload);
        // _testUtilities.ExecutePostRequest<TodoItemModel>("TodoItems", payload);
        
        // act
        var (responseBody, statusCode) = _testUtilities.ExecuteGetRequest<List<TodoItemModel>>(endpoint);
        Console.WriteLine(JsonConvert.SerializeObject(responseBody));
        
        // assert
        statusCode.Should().Be(HttpStatusCode.OK);
        responseBody.Should().NotBeNullOrEmpty();
    }
}