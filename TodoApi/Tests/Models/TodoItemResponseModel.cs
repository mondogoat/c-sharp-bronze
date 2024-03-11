namespace Tests.Models;

public class TodoItemResponseModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }
    public DateTime CreatedTime { get; set; }
    public DateTime? CompletedTime { get; set; }
}