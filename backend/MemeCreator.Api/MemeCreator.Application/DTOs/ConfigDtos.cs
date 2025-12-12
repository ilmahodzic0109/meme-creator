using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemeCreator.Application.DTOs
{
    public class ConfigDtos
    {
        //defaultne vrijednosti
        public string TopText { get; set; } = string.Empty;
        public string BottomText { get; set; } = string.Empty;

        public string FontFamily { get; set; } = "Impact";
        public int FontSize { get; set; } = 40;

        public string TextColor { get; set; } = "#FFFFFF";
        public string StrokeColor { get; set; } = "#000000";
        public int StrokeWidth { get; set; } = 3;

        public string TextAlign { get; set; } = "center";
        public int Padding { get; set; } = 10;
        public bool AllCaps { get; set; } = true;

        public string? WatermarkImageBase64 { get; set; }
        public string? WatermarkPosition { get; set; }

        public float ScaleDown { get; set; } = 0.1f; // <= 0.25
    }

    public class UpdateConfigRequest : ConfigDtos { }

    public class ConfigResponse
    {
        public int Id { get; set; }

        public string TopText { get; set; } = string.Empty;
        public string BottomText { get; set; } = string.Empty;

        public string FontFamily { get; set; } = string.Empty;
        public int FontSize { get; set; }

        public string TextColor { get; set; } = string.Empty;
        public string StrokeColor { get; set; } = string.Empty;
        public int StrokeWidth { get; set; }

        public string TextAlign { get; set; } = string.Empty;
        public int Padding { get; set; }
        public bool AllCaps { get; set; }

        public string? WatermarkPosition { get; set; }
        public float ScaleDown { get; set; }

        public string? WatermarkImageBase64 { get; set; }
    }
}
