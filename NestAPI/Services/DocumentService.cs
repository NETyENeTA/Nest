using NestAPI.Data;
using NestAPI.Entities.DTOs.Document;
using NestAPI.Interfaces;
using NestAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace NestAPI.Services;

public class DocumentService : IDocumentService
{
    private readonly DateBaseContext _db;
    public DocumentService(DateBaseContext db) => _db = db;

    public async Task<IEnumerable<DocumentResponse>> GetDocumentsAsync(string userId, string role)
    {
        var query = _db.Documents.Include(d => d.Owner).AsNoTracking();
        if (role == "Manager") query = query.Where(d => d.IsPublic || d.OwnerId == userId);
        else if (role == "User") query = query.Where(d => d.IsPublic);

        return await query.Select(d => new DocumentResponse
        {
            Id = d.Id,
            Title = d.Title,
            Content = d.Content,
            IsPublic = d.IsPublic,
            OwnerName = d.Owner.Username,
            CreatedAt = d.CreatedAt
        }).ToListAsync();
    }

    public async Task<DocumentResponse> CreateAsync(DocumentCreateRequest request, string userId)
    {
        var doc = new Document { Title = request.Title, Content = request.Content, IsPublic = request.IsPublic, OwnerId = userId };
        _db.Documents.Add(doc);
        await _db.SaveChangesAsync();
        var owner = await _db.Users.FirstAsync(u => u.Id == userId);
        return new DocumentResponse { Id = doc.Id, Title = doc.Title, Content = doc.Content, IsPublic = doc.IsPublic, OwnerName = owner.Username, CreatedAt = doc.CreatedAt };
    }

    public async Task<bool> DeleteAsync(int id, string userId, string role)
    {
        var doc = await _db.Documents.FindAsync(id);
        if (doc == null || (role != "Admin" && doc.OwnerId != userId)) return false;
        doc.IsDeleted = true;
        await _db.SaveChangesAsync();
        return true;
    }
}