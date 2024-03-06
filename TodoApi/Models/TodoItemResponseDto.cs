using System.Text.Json.Serialization;

namespace TodoApi.Models;

public class TodoItemResponseDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }
    public DateTime CreatedTime { get; set; }

    public DateTime? CompletedTime { get; set; }
}