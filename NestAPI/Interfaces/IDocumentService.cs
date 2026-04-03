using NestAPI.Entities;
using NestAPI.Entities.DTOs.Document;

namespace NestAPI.Interfaces;

public interface IDocumentService
{
    Task<IEnumerable<DocumentResponse>> GetDocumentsAsync(string userId, string role);
    Task<DocumentResponse> CreateAsync(DocumentCreateRequest request, string userId);
    Task<bool> DeleteAsync(int id, string userId, string role);

}
