using RestSharp;
using Tests.Models;

namespace Tests.TestFixtures;

public class BaseTestFixture
{
    protected Helpers.Helpers _helpers;
    protected readonly string Endpoint = "TodoItems";
    private RestClient? _client;
    
    [OneTimeSetUp]
    public virtual void Setup()
    {
        _client = new RestClient("http://localhost:8080/api");
        _helpers = new Helpers.Helpers(_client.Options.BaseUrl.ToString());
        
        if (_helpers == null)
        {
            throw new InvalidOperationException("Helpers not initialized");
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
        var (todoItems, _) = _helpers.ExecuteGetRequest<List<TodoItemResponseModel>>(Endpoint);
    
        if (todoItems.Any())
        {
            foreach (var todoItem in todoItems)
            {
                _helpers.ExecuteDeleteRequest(Endpoint, todoItem.Id);
            }
        }
    }
}