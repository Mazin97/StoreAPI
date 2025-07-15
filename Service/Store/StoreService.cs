using Domain.Interfaces;
using Domain.Models;
using System.Text;
using System.Text.Json;

namespace Service.Store;

public class StoreService(IStoreRepository repository,
    IAuthorizationService authService,
    INotificationService notificationService) : IStoreService
{
    private readonly IStoreRepository _repository = repository;
    private readonly IAuthorizationService _authService = authService;
    private readonly INotificationService _notificationService = notificationService;

    public async Task<User> CreateUserAsync(User user)
    {
        user.Validate();
        user.HashPassword();

        var existingUser = await _repository.FindUserByDocumentOrEmailAsync(user.Document, user.Email);
        if (existingUser != null) throw new InvalidOperationException("User with same document or email already exists.");

        return await _repository.CreateUserAsync(user);
    }

    public async Task TransferAsync(decimal value, int payerId, int payeeId)
    {
        var payer = await _repository.GetAsync(payerId);
        var payee = await _repository.GetAsync(payeeId);

        Transfer.Validate(payer, payee, value);

        bool isTransferAuthorized = await _authService.IsTransferAuthorizedAsync();
        if (isTransferAuthorized == false)
        {
            throw new InvalidOperationException("Transfer is not authorized. Please try again later");
        }

        payer.RemoveBalance(value);
        payee.AddBalance(value);

        var isTransferSuccessfull = await _repository.TransferAsync(payer, payee);
        if (isTransferSuccessfull == false)
        {
            throw new InvalidOperationException("An error ocurred while transfering. Please try again later");
        }

        await NotifyUsersAboutTransferAsync(payer, payee, value);
    }

    public async Task DepositAsync(Deposit deposit)
    {
        deposit.Validate();

        var user = await _repository.FindUserByDocumentOrEmailAsync(deposit.Document, deposit.Email) ?? throw new ArgumentNullException("User not found.");

        user.CheckPassword(deposit.Password);
        user.AddBalance(deposit.Value);

        await _repository.UpdateAsync(user);
    }

    private async Task<bool> NotifyUsersAboutTransferAsync(User payer, User payee, decimal value)
    {
        var payerNotification = Notification.CreateTransferEmailNotification(
            payer.Email,
            $"Your transfer to {payee.Name} of R${value} was successfully realized."
        );

        var payeeNotification = Notification.CreateTransferEmailNotification(
            payee.Email,
            $"You received a transfer of R${value} from {payer.Name}."
        );

        var payerNotificationTask = _notificationService.SendNotificationAsync(payerNotification);
        var payeeNotificationTask = _notificationService.SendNotificationAsync(payeeNotification);

        var results = await Task.WhenAll(payerNotificationTask, payeeNotificationTask);

        return results.All(_ => _ == true);
    }
}
