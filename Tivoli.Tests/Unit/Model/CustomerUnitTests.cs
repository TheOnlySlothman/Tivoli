using Tivoli.Models.Entity;

namespace Tivoli.AdminTests.Unit.Model;

public class CustomerUnitTests
{
    [Fact]
    public void ConstructCustomer()
    {
        // Arrange
        string data = string.Join("", Enumerable.Range(0, 1024).Select(x => (char)(byte)x));;

        Card card = new(data);

        Guid guid = Guid.NewGuid();
        // Act
        Customer customer = new()
        {
            AccessFailedCount = 0,
            ConcurrencyStamp = null,
            Email = "Test@Test.dk",
            EmailConfirmed = false,
            Id = guid,
            LockoutEnabled = false,
            LockoutEnd = null,
            NormalizedEmail = null,
            NormalizedUserName = null,
            PasswordHash = null,
            PhoneNumber = null,
            PhoneNumberConfirmed = false,
            SecurityStamp = null,
            TwoFactorEnabled = false,
            UserName = "TestUser"
        };

        customer.Cards.Add(card);
        // Assert
        Assert.Contains(card, customer.Cards);
        Assert.Equal("Test@Test.dk", customer.Email);
        Assert.Equal("TestUser", customer.UserName);
        Assert.Equal(guid, customer.Id);
    }
}