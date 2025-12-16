using MemeCreator.Application.DTOs;
using MemeCreator.Domain.Entities;

namespace MemeCreator.Application.Mapping;

public static class ConfigRequestMapping
{
    public static MemeConfig ToEntity(this ConfigDtos request)
    {
        var entity = new MemeConfig
        {
            TopText = request.TopText,
            BottomText = request.BottomText,
            FontFamily = request.FontFamily,
            FontSize = request.FontSize,
            TextColor = request.TextColor,
            StrokeColor = request.StrokeColor,
            StrokeWidth = request.StrokeWidth,
            TextAlign = request.TextAlign,
            Padding = request.Padding,
            AllCaps = request.AllCaps,
            WatermarkPosition = request.WatermarkPosition,
            ScaleDown = request.ScaleDown
        };

        entity.WatermarkImage = DecodeBase64OrNull(request.WatermarkImageBase64);
        return entity;
    }

    public static void ApplyTo(this UpdateConfigRequest request, MemeConfig entity)
    {
        entity.TopText = request.TopText;
        entity.BottomText = request.BottomText;
        entity.FontFamily = request.FontFamily;
        entity.FontSize = request.FontSize;
        entity.TextColor = request.TextColor;
        entity.StrokeColor = request.StrokeColor;
        entity.StrokeWidth = request.StrokeWidth;
        entity.TextAlign = request.TextAlign;
        entity.Padding = request.Padding;
        entity.AllCaps = request.AllCaps;
        entity.WatermarkPosition = request.WatermarkPosition;
        entity.ScaleDown = request.ScaleDown;

        entity.WatermarkImage = DecodeBase64OrNull(request.WatermarkImageBase64);
    }

    private static byte[]? DecodeBase64OrNull(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return null;

        var comma = input.IndexOf(',');
        var base64 = comma >= 0 ? input[(comma + 1)..] : input;

        try
        {
            return Convert.FromBase64String(base64);
        }
        catch
        {
            return null;
        }
    }

}
