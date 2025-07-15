using Domain.Enums;
using Domain.Models;

namespace StoreAPI.Tests.Domain;

[TestClass]
public class NotificationTests
{
    [TestMethod]
    public async Task Notification_CreateTransferEmailNotification_Valid()
    {
        // Arrange
        var message = "my notification";

        // Act
        var notification = Notification.CreateTransferEmailNotification("mailsample@test.com", message);

        // Assert
        Assert.IsNotNull(notification);
        Assert.IsTrue(notification.Message.Equals(message));
        Assert.IsTrue(notification.Type == NotificationType.Email);
    }
}
