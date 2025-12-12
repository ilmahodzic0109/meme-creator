using MemeCreator.Application.DTOs;

namespace MemeCreator.Application.Interfaces;

public interface IConfigService
{
    Task<ConfigResponse> CreateAsync(ConfigDtos request);
    Task<ConfigResponse> UpdateAsync(int id, UpdateConfigRequest request);
}
