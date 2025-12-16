using MemeCreator.Application.Interfaces;
using MemeCreator.Domain.Entities;
using SkiaSharp;

namespace MemeCreator.Application.Services;

public class MemeService : IMemeService
{
    private readonly IConfigRepository _configRepo;

    public MemeService(IConfigRepository configRepo)
    {
        _configRepo = configRepo;
    }
    
    public async Task<byte[]> PreviewAsync(byte[] imageBytes, int configId, int? canvasWidth, int? canvasHeight)
    {
        var config = await LoadConfig(configId);
        return Render(imageBytes, config, isPreview: true, canvasWidth, canvasHeight);
    }

    public async Task<byte[]> GenerateAsync(byte[] imageBytes, int configId, int? canvasWidth, int? canvasHeight)
    {
        var config = await LoadConfig(configId);
        return Render(imageBytes, config, isPreview: false, canvasWidth, canvasHeight);
    }

    private byte[] Render(byte[] imageBytes, MemeConfig config, bool isPreview, int? canvasWidth, int? canvasHeight)
    {
        using var inputStream = new SKMemoryStream(imageBytes);
        using var originalBitmap = SKBitmap.Decode(inputStream);

        if (originalBitmap == null)
            throw new ArgumentException("Invalid image file.");

        int targetW = canvasWidth ?? originalBitmap.Width;
        int targetH = canvasHeight ?? originalBitmap.Height;

        if (targetW <= 0 || targetH <= 0)
            throw new ArgumentException("CanvasWidth/CanvasHeight must be > 0.");

        using var baseBitmap = ResizeTo(originalBitmap, targetW, targetH);

        float scale = isPreview ? config.ScaleDown : 1f;
        if (scale <= 0 || scale > 1f) scale = 1f;

        int outW = Math.Max(1, (int)Math.Round(baseBitmap.Width * scale));
        int outH = Math.Max(1, (int)Math.Round(baseBitmap.Height * scale));

        using var finalBitmap = (scale == 1f) ? baseBitmap.Copy() : ResizeTo(baseBitmap, outW, outH);

        using var surface = SKSurface.Create(new SKImageInfo(finalBitmap.Width, finalBitmap.Height));
        var canvas = surface.Canvas;

        canvas.Clear(SKColors.Transparent);
        canvas.DrawBitmap(finalBitmap, 0, 0);

        int padding = (int)Math.Round(config.Padding * scale);
        int fontSize = (int)Math.Round(config.FontSize * scale);
        int strokeWidth = Math.Max(1, (int)Math.Round(config.StrokeWidth * scale));

        
        string top = (config.TopText ?? string.Empty).Trim();
        string bottom = (config.BottomText ?? string.Empty).Trim();
        if (config.AllCaps)
        {
            top = top.ToUpperInvariant();
            bottom = bottom.ToUpperInvariant();
        }

        DrawCaption(canvas, finalBitmap.Width, finalBitmap.Height, top, config, fontSize, strokeWidth, padding, isTop: true);
        DrawCaption(canvas, finalBitmap.Width, finalBitmap.Height, bottom, config, fontSize, strokeWidth, padding, isTop: false);

        if (config.WatermarkImage != null && config.WatermarkImage.Length > 0)
        {
            DrawWatermark(canvas, finalBitmap.Width, finalBitmap.Height, config, scale, padding);
        }

        using var image = surface.Snapshot();
        using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        return data.ToArray();
    }

    private static SKBitmap ResizeTo(SKBitmap src, int w, int h)
    {
        var info = new SKImageInfo(w, h);
        var resized = new SKBitmap(info);
        using var canvas = new SKCanvas(resized);
        canvas.Clear(SKColors.Transparent);

        var paint = new SKPaint { FilterQuality = SKFilterQuality.High };
        canvas.DrawBitmap(src, new SKRect(0, 0, w, h), paint);
        return resized;
    }

    private static void DrawCaption(
        SKCanvas canvas,
        int imgW,
        int imgH,
        string text,
        MemeConfig config,
        int fontSize,
        int strokeWidth,
        int padding,
        bool isTop)
    {
        if (string.IsNullOrWhiteSpace(text) || fontSize <= 0) return;

        var typeface = SKTypeface.FromFamilyName(config.FontFamily) ?? SKTypeface.Default;

        var fillPaint = new SKPaint
        {
            Typeface = typeface,
            TextSize = fontSize,
            Color = ParseColor(config.TextColor, SKColors.White),
            IsAntialias = true,
            Style = SKPaintStyle.Fill
        };

        var strokePaint = new SKPaint
        {
            Typeface = typeface,
            TextSize = fontSize,
            Color = ParseColor(config.StrokeColor, SKColors.Black),
            IsAntialias = true,
            Style = SKPaintStyle.Stroke,
            StrokeWidth = strokeWidth,
            StrokeJoin = SKStrokeJoin.Round
        };

        float maxWidth = imgW - 2f * padding;
        if (maxWidth <= 10) return;

        var lines = WrapText(text, fillPaint, maxWidth);

        fillPaint.GetFontMetrics(out var metrics);
        float lineHeight = (metrics.Descent - metrics.Ascent) + (fontSize * 0.15f);

        float totalHeight = lines.Count * lineHeight;

        float y = isTop
            ? padding - metrics.Ascent
            : (imgH - padding - totalHeight) - metrics.Ascent;

        foreach (var line in lines)
        {
            float x = GetAlignedX(line, fillPaint, imgW, padding, config.TextAlign);

            canvas.DrawText(line, x, y, strokePaint);
            canvas.DrawText(line, x, y, fillPaint);

            y += lineHeight;
        }
    }

    private static float GetAlignedX(string line, SKPaint paint, int imgW, int padding, string? align)
    {
        float textWidth = paint.MeasureText(line);
        align = (align ?? "center").Trim().ToLowerInvariant();

        return align switch
        {
            "left" => padding,
            "right" => imgW - padding - textWidth,
            _ => (imgW - textWidth) / 2f
        };
    }

    private static List<string> WrapText(string text, SKPaint paint, float maxWidth)
    {
        var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var lines = new List<string>();
        var current = "";

        foreach (var w in words)
        {
            var test = string.IsNullOrEmpty(current) ? w : current + " " + w;
            if (paint.MeasureText(test) <= maxWidth)
            {
                current = test;
            }
            else
            {
                if (!string.IsNullOrEmpty(current))
                    lines.Add(current);

                current = w;
            }
        }

        if (!string.IsNullOrEmpty(current))
            lines.Add(current);

        return lines;
    }

    private static void DrawWatermark(SKCanvas canvas, int imgW, int imgH, MemeConfig config, float scale, int padding)
    {
        using var wmStream = new SKMemoryStream(config.WatermarkImage);
        using var wmBitmap = SKBitmap.Decode(wmStream);
        if (wmBitmap == null) return;

        float wmTargetW = Math.Max(30, imgW * 0.15f);
        float ratio = wmTargetW / wmBitmap.Width;
        int wmW = (int)Math.Round(wmBitmap.Width * ratio);
        int wmH = (int)Math.Round(wmBitmap.Height * ratio);

        using var resized = ResizeTo(wmBitmap, wmW, wmH);

        string pos = (config.WatermarkPosition ?? "bottom-right").Trim().ToLowerInvariant();

        float x = pos.Contains("left") ? padding : imgW - padding - resized.Width;
        float y = pos.Contains("top") ? padding : imgH - padding - resized.Height;

        canvas.DrawBitmap(resized, x, y);
    }

    private static SKColor ParseColor(string? input, SKColor fallback)
    {
        if (string.IsNullOrWhiteSpace(input)) return fallback;

        input = input.Trim();

        if (input.StartsWith("#") && SKColor.TryParse(input, out var sk))
            return sk;

        if (input.StartsWith("rgba", StringComparison.OrdinalIgnoreCase))
        {
            try
            {
                var inside = input.Substring(input.IndexOf('(') + 1);
                inside = inside.TrimEnd(')');
                var parts = inside.Split(',', StringSplitOptions.TrimEntries);

                byte r = byte.Parse(parts[0]);
                byte g = byte.Parse(parts[1]);
                byte b = byte.Parse(parts[2]);
                float a = float.Parse(parts[3], System.Globalization.CultureInfo.InvariantCulture);
                byte alpha = (byte)Math.Clamp((int)Math.Round(a * 255), 0, 255);

                return new SKColor(r, g, b, alpha);
            }
            catch { return fallback; }
        }

        return fallback;
    }

    private async Task<MemeConfig> LoadConfig(int id)
    {
        var config = await _configRepo.GetByIdAsync(id);
        if (config == null) throw new KeyNotFoundException("Config not found.");

        if (config.ScaleDown > 0.25f) throw new ArgumentException("scaleDown must be <= 0.25");
        if (config.ScaleDown <= 0f) throw new ArgumentException("scaleDown must be > 0");

        return config;
    }
}
