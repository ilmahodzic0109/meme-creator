using MemeCreator.Domain.Entities;

namespace MemeCreator.Application.Interfaces;

public interface IConfigRepository
{
    Task<MemeConfig> AddAsync(MemeConfig config);
    Task<MemeConfig?> GetByIdAsync(int id);
    Task SaveChangesAsync();
}
