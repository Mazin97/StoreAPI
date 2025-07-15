using Domain.Models;

namespace Domain.Interfaces;

public interface IStoreService
{
    Task<User> CreateUserAsync(User user);

    Task AddBalance(decimal value, int userId);

    Task TransferAsync(decimal value, int payerId, int payeeId);
}
