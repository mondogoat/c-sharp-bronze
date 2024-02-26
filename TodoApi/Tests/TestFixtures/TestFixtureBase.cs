using System.Net;
using Newtonsoft.Json;
using RestSharp;
using Tests.Models;

namespace Tests.TestFixtures;

public class TestFixtureBase
{
    protected (T, HttpStatusCode) ExecuteGetRequest<T>(string endpoint)
    {
        var client = new RestClient(endpoint);
        var request = new RestRequest();
        request.Method = Method.Get;

        var response = client.Execute(request);
        var responseBody = JsonConvert.DeserializeObject<T>(response.Content);
        return (responseBody, response.StatusCode);
    }

    protected (T, HttpStatusCode) ExecutePostRequest<T>(string endpoint)
    {
        // TODO add id as a value returned by the method
        
        var client = new RestClient(endpoint);
        var request = new RestRequest();
        request.Method = Method.Post;

        var payload = new TodoItemModel
        {
            id = 999,
            name = "test3",
            isComplete = false
        };
        request.AddJsonBody(payload);
        
        var response = client.Execute(request);
        var responseBody = JsonConvert.DeserializeObject<T>(response.Content);
        
        return (responseBody, response.StatusCode);
    }

    protected (T, HttpStatusCode) ExecuteGetOneRequest<T>(string endpoint, int id)
    {
        var client = new RestClient($"{endpoint}/{id}");
        var request = new RestRequest();
        request.Method = Method.Get;

        var response = client.Execute(request);
        
        // if (response.StatusCode != HttpStatusCode.OK)
        // {
        //     throw new Exception($"Request failed with status code: {response.StatusCode}");
        // }

        var responseBody = JsonConvert.DeserializeObject<T>(response.Content);

        return (responseBody, response.StatusCode);
    }
    
    protected (T, HttpStatusCode) ExecutePutRequest<T>(string endpoint, int id)
    {
        // TODO add setup in which a test will create a todo item first, then return that id and use it here
        var client = new RestClient($"{endpoint}/{id}");
        var request = new RestRequest();
        request.Method = Method.Put;
        
        var payload = new TodoItemModel
        {
            id = 1,
            name = "edit test",
            isComplete = true
        };

        request.AddJsonBody(payload);
        
        var response = client.Execute(request);
        var responseBody = JsonConvert.DeserializeObject<T>(response.Content);
        
        return (responseBody, response.StatusCode);
    }
    
    protected HttpStatusCode ExecuteDeleteRequest(string endpoint, int id)
    {
        // TODO add setup in which a test will create a todo item first, then return that id and use it here
        // is there a need to return responseBody? 
        var client = new RestClient($"{endpoint}/{id}");
        var request = new RestRequest();
        request.Method = Method.Delete;
        
        var response = client.Execute(request);

        return response.StatusCode;
    }
}