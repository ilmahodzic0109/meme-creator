using Microsoft.AspNetCore.Mvc;
using MemeCreator.Application.Interfaces;
using MemeCreator.Api.DTOs;

namespace MemeCreator.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MemeController : ControllerBase
{
    private readonly IMemeService _service;

    public MemeController(IMemeService service)
    {
        _service = service;
    }

    [HttpPost("preview")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Preview([FromForm] MemeFormDto request)
    {
        if (request.Image == null || request.Image.Length == 0)
            return BadRequest("Image is required.");

        await using var ms = new MemoryStream();
        await request.Image.CopyToAsync(ms);

        var bytes = await _service.PreviewAsync(
            ms.ToArray(), request.ConfigId, request.CanvasWidth, request.CanvasHeight);

        return File(bytes, "image/png");
    }

    [HttpPost("generate")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Generate([FromForm] MemeFormDto request)
    {
        if (request.Image == null || request.Image.Length == 0)
            return BadRequest("Image is required.");

        await using var ms = new MemoryStream();
        await request.Image.CopyToAsync(ms);

        var bytes = await _service.GenerateAsync(
            ms.ToArray(), request.ConfigId, request.CanvasWidth, request.CanvasHeight);

        return File(bytes, "image/png");
    }
}
