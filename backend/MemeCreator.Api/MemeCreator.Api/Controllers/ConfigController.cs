using Microsoft.AspNetCore.Mvc;
using MemeCreator.Application.DTOs;
using MemeCreator.Application.Interfaces;

namespace MemeCreator.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConfigController : ControllerBase
{
    private readonly IConfigService _service;

    public ConfigController(IConfigService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<ConfigResponse>> Create([FromBody] ConfigDtos request)
    {
        try
        {
            var result = await _service.CreateAsync(request);
            return Created(string.Empty, result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ConfigResponse>> Update(int id, [FromBody] UpdateConfigRequest request)
    {
        try
        {
            var result = await _service.UpdateAsync(id, request);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
