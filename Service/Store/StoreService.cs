using Domain.Models;
using Domain.Interfaces;

namespace Service.Store;

public class StoreService : IStoreService
{
    private readonly IStoreRepository _repository;

    public StoreService(IStoreRepository repository)
    {
        _repository = repository;
    }

    public Task AddBalance(double value, int userId)
    {
        throw new NotImplementedException();
    }

    public async Task<User> CreateUserAsync(User user)
    {
        user.Validate();

        return await _repository.CreateUserAsync(user);
    }

    public Task Transfer(double value, int payerId, int payeeId)
    {
        throw new NotImplementedException();
    }
}
