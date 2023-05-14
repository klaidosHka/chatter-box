using ChatterBox.Interfaces.Services;
using Imgur.API.Authentication;
using Imgur.API.Endpoints;


namespace ChatterBox.Services.Services;

public class ImageService : IImageService
{
    private readonly string _clientId;
    private readonly string _clientSecret;

    public ImageService(string clientId, string clientSecret)
    {
        _clientId = clientId;
        _clientSecret = clientSecret;
    }

    public async Task<string> UploadAsync(byte[] fileBytes, string fileName)
    {
        var clientApi = new ApiClient(_clientId, _clientSecret);
        var clientHttp = new HttpClient();
        var imageEndpoint = new ImageEndpoint(clientApi, clientHttp);

        using var ms = new MemoryStream(fileBytes);

        return (await imageEndpoint.UploadImageAsync(ms)).Link;
    }
}