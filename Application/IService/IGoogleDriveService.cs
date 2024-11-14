using Microsoft.AspNetCore.Http;

namespace Application.IService;

public interface IGoogleDriveService
{
    public Task<string> UploadFileAsync(IFormFile file);
}