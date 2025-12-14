namespace MemeCreator.Api.DTOs;

public class MemeFormDto
{
    public int ConfigId { get; set; }
    public int? CanvasWidth { get; set; }
    public int? CanvasHeight { get; set; }
    public IFormFile Image { get; set; } = default!;
}
