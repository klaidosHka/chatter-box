namespace ChatterBox.Interfaces.Services;

public interface IImageService
{
    public Task<string> UploadAsync(Stream stream, string fileName);
}