using System.Net;
using FluentAssertions;
using Newtonsoft.Json;
using RestSharp;
using Tests.Models;

namespace Tests.TestFixtures;

[TestFixture, Order(2)]
public class GetOneTodoItemTests
{
    private readonly Helpers _helpers;
    private readonly string _endpoint = "TodoItems";
    public GetOneTodoItemTests()
    {
        _helpers = new Helpers("http://localhost:8080/api");
    }

    [Test]
    public void GetOneTodoItem_Success()
    {
        // arrange
        var payload = new TodoItemRequestModel()
        {
            Name = "test get one",
            IsComplete = false
        };
        var createdId = _helpers.GenerateId(payload);

        // act
        var (responseBody, statusCode) = _helpers.ExecuteGetOneRequest<TodoItemResponseModel>(_endpoint, createdId);

        // assert
        statusCode.Should().Be(HttpStatusCode.OK);
        responseBody.Should().NotBeNull();
    }

    [Test]
    public void GetOneTodoItem_NonExistentId()
    {
        // arrange
        
        // act
        var (responseBody, statusCode) = _helpers.ExecuteGetOneRequest<TodoItemResponseModel>(_endpoint, 1234123);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public void GetOneTodoItem_InvalidId()
    {
        // arrange
        
        // act
        var (responseBody, statusCode) = _helpers.ExecuteGetOneRequest<TodoItemResponseModel>(_endpoint, "abc");
        
        // assert
        statusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
}