using Domain.Models;
using Domain.Interfaces;

namespace Service.Store;

public class StoreService(IStoreRepository repository) : IStoreService
{
    private readonly IStoreRepository _repository = repository;

    public Task AddBalance(double value, int userId)
    {
        throw new NotImplementedException();
    }

    public async Task<User> CreateUserAsync(User user)
    {
        user.Validate();
        user.HashPassword();

        var existingUser = await _repository.FindUserByDocumentOrEmailAsync(user.Document, user.Email);
        if (existingUser != null) throw new InvalidOperationException("User with same document or email already exists.");

        return await _repository.CreateUserAsync(user);
    }

    public Task Transfer(double value, int payerId, int payeeId)
    {
        throw new NotImplementedException();
    }
}
