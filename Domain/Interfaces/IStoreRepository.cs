using Domain.Models;

namespace Domain.Interfaces;

public interface IStoreRepository
{
    Task<User> CreateUserAsync(User user);
}
