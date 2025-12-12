using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MemeCreator.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MemeConfigs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TopText = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    BottomText = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FontFamily = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FontSize = table.Column<int>(type: "int", nullable: false),
                    TextColor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StrokeColor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StrokeWidth = table.Column<int>(type: "int", nullable: false),
                    TextAlign = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Padding = table.Column<int>(type: "int", nullable: false),
                    AllCaps = table.Column<bool>(type: "bit", nullable: false),
                    WatermarkImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    WatermarkPosition = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ScaleDown = table.Column<float>(type: "real", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemeConfigs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemeConfigs");
        }
    }
}
