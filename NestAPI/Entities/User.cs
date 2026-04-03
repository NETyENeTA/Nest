using System.Reflection.Metadata;

namespace NestAPI.Entities;

public class User
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string Role { get; set; } = "User";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    public ICollection<Document> Documents { get; set; } = new List<Document>();
}