using Domain.Interfaces;
using Domain.Models;
using System.Text;
using System.Text.Json;

namespace Service.Store;
public class NotificationService(string utilsServiceURL, int maxRetries) : INotificationService
{
    private readonly string _utilsServiceURL = utilsServiceURL;

    public async Task<bool> SendNotificationAsync(Notification notification)
    {
        using (var client = new HttpClient())
        {
            for (int i = 0; i < maxRetries; i++)
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
