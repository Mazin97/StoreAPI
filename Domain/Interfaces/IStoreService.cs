using Domain.Models;

namespace Domain.Interfaces;

public interface IStoreService
{
    Task<User> CreateUserAsync(User user);

    Task DepositAsync(Deposit deposit);

    Task TransferAsync(decimal value, int payerId, int payeeId);
}
