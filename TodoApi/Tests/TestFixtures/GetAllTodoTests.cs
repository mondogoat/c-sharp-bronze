using System.Net;
using Newtonsoft.Json;
using RestSharp;
using FluentAssertions;
using Tests.Models;

namespace Tests.TestFixtures;

public class GetAllTodoTests
{
    private readonly TestUtilities _testUtilities;

    public GetAllTodoTests()
    {
        _testUtilities = new TestUtilities("http://localhost:8080/api");
    }

    [Test]
    public void GetAllTodoSuccess()
    {
        // arrange
        var endpoint = "TodoItems";
        // var payload = new TodoItemModel()
        // {
        //     
        // }
        // _testUtilities.ExecutePostRequest<TodoItemModel>(payload);
        
        // act
        var (responseBody, statusCode) = _testUtilities.ExecuteGetRequest<List<TodoItemModel>>(endpoint);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.OK);
        responseBody.Should().NotBeNullOrEmpty();
    }
}