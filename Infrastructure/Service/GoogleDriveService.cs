using System.Text;
using Application.IService;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Upload;
using Infrastructure.Settings;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using File = Google.Apis.Drive.v3.Data.File;

namespace Infrastructure.Service;

public class GoogleDriveService(GoogleDriveSetting googleDriveSetting) : IGoogleDriveService
{
    private readonly GoogleDriveSetting _googleDriveSetting = googleDriveSetting;

    public async Task<string> UploadFileAsync(IFormFile file)
    {
        //set param
        string[] scopes = [DriveService.Scope.DriveFile];
        GoogleCredential credential;
        var jsonSettingContent = JsonConvert.SerializeObject(new
        {
            type = _googleDriveSetting.Type,
            project_id = _googleDriveSetting.ProjectId,
            private_key_id = _googleDriveSetting.PrivateKeyId,
            private_key = _googleDriveSetting.PrivateKey,
            client_email = _googleDriveSetting.ClientEmail,
            client_id = _googleDriveSetting.ClientId,
            auth_uri = _googleDriveSetting.AuthUri,
            token_uri = _googleDriveSetting.TokenUri,
            auth_provider_x509_cert_url = _googleDriveSetting.AuthProviderUrl,
            client_x509_cert_url = _googleDriveSetting.ClientCert,
            universe_domain = _googleDriveSetting.UniverseDomain,
        });

        var ParentFolder = Directory.GetParent(Directory.GetCurrentDirectory());
        var JsonFile = Path.Combine(ParentFolder.FullName, "LearnEnglishApi", $"GGDrive.json");
        credential = GoogleCredential.FromFile(JsonFile).CreateScoped(scopes);
        // credential = GoogleCredential.FromStream(ggSettingStream).CreateScoped(scopes);

        var service = new DriveService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = _googleDriveSetting.ProjectName
        });

        var fileMetadata = new File()
        {
            Name = file.FileName,
            MimeType = file.ContentType,
            Parents = new[] { "1puhaQzZq9xDuVrjLoy1Ih5eQqernRWdQ" }
        };

        string fileId;
        await using var stream = file.OpenReadStream();
        var request = service.Files.Create(fileMetadata, stream, file.ContentType);
        request.Fields = "id";
        // upload
        var fileGGdrive = await request.UploadAsync();
        if (fileGGdrive.Status != UploadStatus.Completed)
        {
            return string.Empty;
        }

        var id = request.ResponseBody.Id;
        // public file for everyone
        var permission = new Permission()
        {
            Role = "reader",
            Type = "anyone"
        };

        await service.Permissions.Create(permission, id).ExecuteAsync();

        return id;
    }
}