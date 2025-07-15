using Domain.Interfaces;
using Domain.Models;
using System.Text;
using System.Text.Json;

namespace Service.Store;

public class StoreService(IStoreRepository repository) : IStoreService
{
    private readonly IStoreRepository _repository = repository;
    private const string _utilsServiceURL = "https://util.devi.tools/api";
    private const int _maxNotificationRetries = 3;

    public Task AddBalance(decimal value, int userId)
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

    public async Task TransferAsync(decimal value, int payerId, int payeeId)
    {
        var payer = await _repository.GetAsync(payerId);
        var payee = await _repository.GetAsync(payerId);

        Transfer.Validate(payer, payee, value);

        bool isTransferAuthorized = await IsTransferAuthorizedAsync();
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

    private static async Task<bool> IsTransferAuthorizedAsync()
    {
        using (var client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync($"{_utilsServiceURL}/v2/authorize");

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                return false;
            }
        }

        return true;
    }

    private static async Task<bool> NotifyUsersAboutTransferAsync(User payer, User payee, decimal value)
    {
        var payerNotification = Notification.CreateTransferEmailNotification(
            payer.Email,
            $"Your transfer to {payee.Name} of R${value} was successfully realized."
        );

        var payeeNotification = Notification.CreateTransferEmailNotification(
            payee.Email,
            $"You received a transfer of R${value} from {payer.Name}."
        );

        var payerNotificationTask = SendNotificationAsync(payerNotification);
        var payeeNotificationTask = SendNotificationAsync(payeeNotification);

        var results = await Task.WhenAll(payerNotificationTask, payeeNotificationTask);

        return results.All(_ => _ == true);
    }

    private static async Task<bool> SendNotificationAsync(Notification notification)
    {
        using (var client = new HttpClient())
        {
            for (int i = 0; i < _maxNotificationRetries; i++)
            {
                var json = JsonSerializer.Serialize(notification);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync($"{_utilsServiceURL}/v1/notify", data);

                try
                {
                    response.EnsureSuccessStatusCode();
                    return true;
                }
                catch (Exception)
                {
                    continue;
                }
            }

            return false;
        }
    }
}
