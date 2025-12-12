using Microsoft.EntityFrameworkCore;
using MemeCreator.Application.Interfaces;
using MemeCreator.Domain.Entities;
using MemeCreator.Infrastructure.Persistence;

namespace MemeCreator.Infrastructure.Repositories;

public class ConfigRepository : IConfigRepository
{
    private readonly MemeDbContext _db;

    public ConfigRepository(MemeDbContext db)
    {
        _db = db;
    }

    public async Task<MemeConfig> AddAsync(MemeConfig config)
    {
        _db.MemeConfigs.Add(config);
        await _db.SaveChangesAsync();
        return config;
    }

    public Task<MemeConfig?> GetByIdAsync(int id)
        => _db.MemeConfigs.FirstOrDefaultAsync(x => x.Id == id);

    public Task SaveChangesAsync()
        => _db.SaveChangesAsync();
}
