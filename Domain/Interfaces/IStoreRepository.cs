using Domain.Models;

namespace Domain.Interfaces;

public interface IStoreRepository
{
    Task<User> GetAsync(int id);

    Task<User> CreateUserAsync(User user);

    Task<User> FindUserByDocumentOrEmailAsync(string document, string email);

    Task<bool> TransferAsync(User payer, User payee);
}
