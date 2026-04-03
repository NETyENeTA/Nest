namespace NestAPI.Entities.DTOs.Document;

public class DocumentCreateRequest
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool IsPublic { get; set; }
}