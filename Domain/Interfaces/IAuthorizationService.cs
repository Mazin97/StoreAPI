namespace Domain.Interfaces;

public interface IAuthorizationService
{
    Task<bool> IsTransferAuthorizedAsync();
}
