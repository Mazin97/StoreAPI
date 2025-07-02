using Domain.Models;

namespace Domain.Interfaces;

public interface IStoreService
{
    Task<User> CreateUserAsync(User user);

    Task AddBalance(double value, int userId);

    Task Transfer(double value, int payerId, int payeeId);
}
