namespace Application.IService;

public interface IAssemblyAIService
{
    public Task<string?> VideoToTextAsync(string videoUrl);
}