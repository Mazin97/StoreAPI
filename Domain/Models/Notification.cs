using Domain.Enums;

namespace Domain.Models;

public class Notification
{
    public string Recipient { get; private set; }

    public string Message { get; private set; }

    public NotificationType Type { get; private set; }

    public static Notification CreateTransferEmailNotification(string recipient, string message)
    {
        return new Notification
        {
            Message = message,
            Recipient = recipient,
            Type = Domain.Enums.NotificationType.Email
        };
    }
}
