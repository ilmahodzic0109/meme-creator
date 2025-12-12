namespace MemeCreator.Domain.Entities;

public class MemeConfig
{
    public int Id { get; set; }

    public string TopText { get; set; } = string.Empty;
    public string BottomText { get; set; } = string.Empty;

    public string FontFamily { get; set; } = "Impact";
    public int FontSize { get; set; }

    public string TextColor { get; set; } = "#FFFFFF";
    public string StrokeColor { get; set; } = "#000000";
    public int StrokeWidth { get; set; }

    public string TextAlign { get; set; } = "center";
    public int Padding { get; set; }
    public bool AllCaps { get; set; }

    public byte[]? WatermarkImage { get; set; }
    public string? WatermarkPosition { get; set; }

    public float ScaleDown { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
