using MemeCreator.Application.DTOs;
using MemeCreator.Domain.Entities;

namespace MemeCreator.Application.Mapping;

public static class ConfigResponseMapping
{
    public static ConfigResponse ToResponse(this MemeConfig entity)
    {
        return new ConfigResponse
        {
            Id = entity.Id,
            TopText = entity.TopText,
            BottomText = entity.BottomText,
            FontFamily = entity.FontFamily,
            FontSize = entity.FontSize,
            TextColor = entity.TextColor,
            StrokeColor = entity.StrokeColor,
            StrokeWidth = entity.StrokeWidth,
            TextAlign = entity.TextAlign,
            Padding = entity.Padding,
            AllCaps = entity.AllCaps,
            WatermarkPosition = entity.WatermarkPosition,
            ScaleDown = entity.ScaleDown,
            WatermarkImageBase64 = entity.WatermarkImage != null
                ? Convert.ToBase64String(entity.WatermarkImage)
                : null
        };
    }
}
