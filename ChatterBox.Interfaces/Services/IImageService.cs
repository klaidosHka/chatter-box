namespace ChatterBox.Interfaces.Services;

public interface IImageService
{
    public Task<string> UploadAsync(byte[] fileBytes, string fileName);
}