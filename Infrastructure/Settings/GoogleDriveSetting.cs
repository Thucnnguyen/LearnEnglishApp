using Newtonsoft.Json;

namespace Infrastructure.Settings;

public class GoogleDriveSetting
{
    public static readonly string ConfigSectionName = "GoogleDriveSetting";
    
    public string ProjectName { get; set; }
    public string Type { get; set; }
    
    public string ProjectId { get; set; }
    
    public string PrivateKeyId { get; set; }
    
    public string PrivateKey { get; set; }
    
    public string ClientEmail { get; set; }
    
    public string ClientId { get; set; }

    public string AuthUri { get; set; }
    
    public string TokenUri { get; set; }

    public string AuthProviderUrl { get; set; }

    public string ClientCert { get; set; }
    
    public string UniverseDomain { get; set; }
}