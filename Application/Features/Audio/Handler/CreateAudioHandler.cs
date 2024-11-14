using Application.Common.Models.Request;
using Application.Infrastructure.IRepository;
using Application.IService;
using Google.Apis.Util.Store;
using MediatR;
using NAudio.Wave;

namespace Application.Features.Audio.Handler;

public class CreateAudioHandler : IRequestHandler<CreateAudioCommand, AudioResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGoogleDriveService _googleDriveService;
    private readonly IAssemblyAIService _assemblyAIService;

    public CreateAudioHandler(IUnitOfWork unitOfWork, 
        IGoogleDriveService googleDriveService, 
        IAssemblyAIService assemblyAiService)
    {
        _unitOfWork = unitOfWork;
        _googleDriveService = googleDriveService;
        _assemblyAIService = assemblyAiService;
    }

    public async Task<AudioResponse> Handle(CreateAudioCommand request, CancellationToken cancellationToken)
    {
        try
        {
            //upload File
            var fileId = await _googleDriveService.UploadFileAsync(request.AudioFile);

            if (string.IsNullOrEmpty(fileId))
            {
                return new AudioResponse(string.Empty, 0,string.Empty,string.Empty);
            }
            //get transcript 
            var transcript = await _assemblyAIService
                .VideoToTextAsync($"https://drive.google.com/uc?id={fileId}");
            await using var reader = new Mp3FileReader(request.AudioFile.OpenReadStream());
            return new AudioResponse(fileId,
                reader.TotalTime.TotalSeconds,
                $"https://drive.google.com/uc?id={fileId}",
                transcript);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return new AudioResponse(string.Empty,0,string.Empty,string.Empty);
        }
        
        
        
    }
}