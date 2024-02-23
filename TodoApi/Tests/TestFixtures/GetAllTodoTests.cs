using System.Net;
using Newtonsoft.Json;
using RestSharp;
using FluentAssertions;
using Tests.Models;

namespace Tests.TestFixtures;

public class GetAllTodoTests : TestFixtureBase
{
    [Test]
    public void GetAllTodoSuccess()
    {
        var endpoint = "http://localhost:8080/api/TodoItems";
        var (responseBody, statusCode) = ExecuteGetRequest<List<TodoItemModel>>(endpoint);
        
        // Validate
        statusCode.Should().Be(HttpStatusCode.OK);
        responseBody.Should().NotBeNullOrEmpty();
    }
}