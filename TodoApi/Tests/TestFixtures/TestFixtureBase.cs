using System.Net;
using Newtonsoft.Json;
using RestSharp;

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
}