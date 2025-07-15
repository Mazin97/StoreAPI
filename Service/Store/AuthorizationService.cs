using Domain.Interfaces;

namespace Service.Store;

public class AuthorizationService(string utilsServiceURL) : IAuthorizationService
{
    private readonly string _utilsServiceURL = utilsServiceURL;

    public async Task<bool> IsTransferAuthorizedAsync()
    {
        using var client = new HttpClient();
        try
        {
            var response = await client.GetAsync($"{_utilsServiceURL}/v2/authorize");
            response.EnsureSuccessStatusCode();

            return true;
        }
        catch
        {
            return false;
        }
    }
}
