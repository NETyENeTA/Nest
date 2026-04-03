using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NestAPI.Entities.DTOs.Document;
using NestAPI.Interfaces;
using System.Security.Claims;

namespace NestAPI.Controllers;


[Authorize]
[ApiController]
[Route("api/documents")]
public class DocumentsController : ControllerBase
{
    private readonly IDocumentService _service;
    public DocumentsController(IDocumentService service) => _service = service;

    private string UId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    private string URole => User.FindFirstValue(ClaimTypes.Role)!;

    [HttpGet] public async Task<IActionResult> Get() => Ok(await _service.GetDocumentsAsync(UId, URole));

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Create(DocumentCreateRequest req) => Ok(await _service.CreateAsync(req, UId));

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id) =>
        await _service.DeleteAsync(id, UId, URole) ? NoContent() : Forbid();
}