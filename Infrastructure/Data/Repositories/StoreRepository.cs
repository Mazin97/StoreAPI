using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data.Context;

namespace Infrastructure.Data.Repositories;

public class StoreRepository(StoreContext context) : IStoreRepository
{
    private readonly StoreContext _context = context;
    

    public async Task<User> CreateUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<User> GetUserByDocumentOrEmailAsync(string document, string email)
    {
        //if (!string.IsNullOrEmpty(document))
        //{
        //    var user = await _context.(_ => _.Document == document);
        //}

        return null;
    }
}