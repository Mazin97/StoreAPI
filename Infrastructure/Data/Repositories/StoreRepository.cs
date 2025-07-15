using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class StoreRepository(StoreContext context) : IStoreRepository
{
    private readonly StoreContext _context = context;

    public Task<User> GetAsync(int id)
    {
        return _context.Users.FirstOrDefaultAsync(_ => _.Id == id);
    }

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

    public async Task<bool> TransferAsync(User payer, User payee)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                _context.Users.UpdateRange(payer, payee);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
    }

    public async Task UpdateAsync(User user)
    {
        _context.Update(user);
        await _context.SaveChangesAsync();
    }
}