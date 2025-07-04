using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

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

    public async Task<User> FindUserByDocumentOrEmailAsync(string document, string email)
    {
        var user = default(User);

        if (!string.IsNullOrEmpty(document))
        {
            user = await _context.Users.FirstOrDefaultAsync(_ => _.Document == document);
            if (user != null) return user;
        }

        if (!string.IsNullOrEmpty(email))
        {
            user = await _context.Users.FirstOrDefaultAsync(_ => _.Email == email);
        }

        return user;
    }
}