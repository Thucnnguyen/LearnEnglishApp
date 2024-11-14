namespace Application.Common.Models.Request;

public record class AudioResponse(string AudioId,double Duration ,string AudioUrl, string? Script);