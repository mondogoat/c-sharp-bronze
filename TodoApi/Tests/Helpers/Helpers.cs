using System.Net;
using Newtonsoft.Json;
using RestSharp;
using Tests.Models;

namespace Tests.Helpers;

public class Helpers
{
    private readonly string _baseUrl;

    public Helpers(string baseUrl)
    {
        _baseUrl = baseUrl;
    }

    private (T?, HttpStatusCode) ExecuteRequest<T>(RestRequest request)
    {
        var client = new RestClient(_baseUrl);
        var response = client.Execute(request);

        T responseBody;
        try
        {
            responseBody =  JsonConvert.DeserializeObject<T>(response.Content);
        }
        catch (Exception ex)
        {
            // Handle deserialization exception
            throw new Exception($"Error deserializing response: {ex.Message}");
        }

        return (responseBody, response.StatusCode);
    }

    public (T?, HttpStatusCode) ExecuteGetRequest<T>(string endpoint)
    {
        var request = new RestRequest(endpoint, Method.Get);
        return ExecuteRequest<T>(request);
    }

    public (T, HttpStatusCode) ExecutePostRequest<T>(string endpoint, object payload)
    {
        var request = new RestRequest(endpoint, Method.Post);
        request.AddJsonBody(payload);
        return ExecuteRequest<T>(request);
    }

    public (T, HttpStatusCode) ExecuteGetOneRequest<T>(string endpoint, object id)
    {
        var request = new RestRequest($"{endpoint}/{id}", Method.Get);
        return ExecuteRequest<T>(request);
    }

    public (T?, HttpStatusCode) ExecutePutRequest<T>(string endpoint, object id, object payload)
    {
        var request = new RestRequest($"{endpoint}/{id}", Method.Put);
        request.AddJsonBody(payload);
        return ExecuteRequest<T>(request);
    }

    public HttpStatusCode ExecuteDeleteRequest(string endpoint, object id)
    {
        var request = new RestRequest($"{endpoint}/{id}", Method.Delete);
        var client = new RestClient(_baseUrl);
        var response = client.Execute(request);
        return response.StatusCode;
    }
    
    public int GenerateId(object payload)
    {
        var (createTodoResponse, _) = ExecutePostRequest<TodoItemResponseModel>("TodoItems", payload);
        var createdId = createTodoResponse.Id;
        return createdId;
    }
}
