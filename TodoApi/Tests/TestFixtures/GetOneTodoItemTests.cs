using System.Net;
using FluentAssertions;
using Newtonsoft.Json;
using RestSharp;
using Tests.Models;

namespace Tests.TestFixtures;

[TestFixture, Order(2)]
public class GetOneTodoItemTests: BaseTestFixture
{
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
        var (responseBody, statusCode) = _helpers.ExecuteGetOneRequest<TodoItemResponseModel>(Endpoint, createdId);

        // assert
        statusCode.Should().Be(HttpStatusCode.OK);
        responseBody.Should().NotBeNull();
    }

    [Test]
    public void GetOneTodoItem_NonExistentId()
    {
        // arrange
        
        // act
        var (responseBody, statusCode) = _helpers.ExecuteGetOneRequest<TodoItemResponseModel>(Endpoint, 1234123);
        
        // assert
        statusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Test]
    public void GetOneTodoItem_InvalidId()
    {
        // arrange
        
        // act
        var (responseBody, statusCode) = _helpers.ExecuteGetOneRequest<TodoItemResponseModel>(Endpoint, "abc");
        
        // assert
        statusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
}