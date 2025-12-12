using MemeCreator.Application.DTOs;
using MemeCreator.Application.Interfaces;
using MemeCreator.Application.Mapping;

namespace MemeCreator.Application.Services;

public class ConfigService : IConfigService
{
    private readonly IConfigRepository _repo;

    public ConfigService(IConfigRepository repo)
    {
        _repo = repo;
    }

    public async Task<ConfigResponse> CreateAsync(ConfigDtos request)
    {
        Validate(request.ScaleDown);

        var entity = request.ToEntity();
        entity.CreatedAt = DateTime.UtcNow;

        var saved = await _repo.AddAsync(entity);
        return saved.ToResponse();
    }

    public async Task<ConfigResponse> UpdateAsync(int id, UpdateConfigRequest request)
    {
        Validate(request.ScaleDown);

        var entity = await _repo.GetByIdAsync(id);
        if (entity == null)
            throw new KeyNotFoundException($"Config with id {id} not found.");

        request.ApplyTo(entity);
        entity.UpdatedAt = DateTime.UtcNow;

        await _repo.SaveChangesAsync();
        return entity.ToResponse();
    }

    private static void Validate(float scaleDown)
    {
        if (scaleDown > 0.25f) throw new ArgumentException("scaleDown must be <= 0.25");
        if (scaleDown <= 0f) throw new ArgumentException("scaleDown must be > 0");
    }
}
