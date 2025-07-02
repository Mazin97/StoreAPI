using Domain.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

public class StoreRepository : IStoreRepository
{
    private readonly IDistributedCache _cache;
    private const string UserPrefix = "user:";

    public StoreRepository(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<User> CreateUserAsync(User user)
    {
        var key = UserPrefix + user.Id;
        var data = JsonSerializer.Serialize(user);
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
        };

        await _cache.SetStringAsync(key, data, options);

        return user;
    }
}
