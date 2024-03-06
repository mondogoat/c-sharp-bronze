using System.Text.Json.Serialization;

namespace TodoApi.Models;

public class TodoItemRequestDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }
}