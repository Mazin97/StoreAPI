using Domain.Models;

namespace Domain.Interfaces;

public interface INotificationService
{
    Task<bool> SendNotificationAsync(Notification notification);
}
