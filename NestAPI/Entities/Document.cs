namespace NestAPI.Entities;

public class Document
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public bool IsPublic { get; set; }
    public bool IsDeleted { get; set; }
    public string OwnerId { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User Owner { get; set; } = null!;
}