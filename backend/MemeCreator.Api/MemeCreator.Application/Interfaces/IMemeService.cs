namespace MemeCreator.Application.Interfaces;
public interface IMemeService
{
    Task<byte[]> PreviewAsync(byte[] imageBytes, int configId, int? canvasWidth, int? canvasHeight);
    Task<byte[]> GenerateAsync(byte[] imageBytes, int configId, int? canvasWidth, int? canvasHeight);
}
