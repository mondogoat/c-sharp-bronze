using RestSharp;

namespace Tests.TestFixtures;

public class BaseTestFixture
{
    protected Helpers.Helpers _helpers;
    protected readonly string Endpoint = "TodoItems";
    private RestClient? _client;

    [OneTimeSetUp]
    public virtual void Setup()
    {
        Console.WriteLine("setup. session started");
        _client = new RestClient("http://localhost:8080/api");
        _helpers = new Helpers.Helpers(_client.Options.BaseUrl.ToString());
    }
    
    [OneTimeTearDown]
    public void Dispose()
    {
        _client?.Dispose();
        Console.WriteLine("teardown. session closed.");
    }
}