namespace MemeCreator.Application.DTOs;

public class MemePreviewRequest
{
    public int ConfigId { get; set; }
    public int? CanvasWidth { get; set; }   
    public int? CanvasHeight { get; set; }  
}

public class MemeGenerateRequest
{
    public int ConfigId { get; set; }
    public int? CanvasWidth { get; set; }
    public int? CanvasHeight { get; set; }
}
