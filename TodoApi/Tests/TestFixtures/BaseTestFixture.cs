using RestSharp;
using Tests.Models;

namespace Tests.TestFixtures;

public class BaseTestFixture
{
    protected Helpers.Helpers Helpers;
    protected readonly string Endpoint = "TodoItems";
    private RestClient? _client;
    
    [OneTimeSetUp]
    public virtual void Setup()
    {
        _client = new RestClient("http://localhost:8080/api");
        // Helpers = new Helpers.Helpers(_client.Options.BaseUrl.ToString());
        if (_client.Options.BaseUrl != null)
        {
            Helpers = new Helpers.Helpers(_client.Options.BaseUrl.ToString());
        }
        else
        {
            throw new NullReferenceException("BaseUrl is null.");
        }
    }
    
    [OneTimeTearDown]
    public void Dispose()
    {
        _client?.Dispose();
    }
    
    [TearDown]
    public void ClearTodoList()
    {
        var (todoItems, _) = Helpers.ExecuteGetRequest<List<TodoItemResponseModel>>(Endpoint);
    
        if (todoItems.Any())
        {
            foreach (var todoItem in todoItems)
            {
                Helpers.ExecuteDeleteRequest(Endpoint, todoItem.Id);
            }
        }
    }
}