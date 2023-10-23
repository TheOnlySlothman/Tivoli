using Tivoli.Dal.Entities;
using Assert = Xunit.Assert;

namespace Tivoli.Dal.Tests.Model;

public class CardUnitTests
{
    [Fact]
    public void ConstructCardWithArray()
    {
        // Arrange
        string data = string.Join("", Enumerable.Range(0, 1024).Select(x => (char)(byte)x));

        Customer customer = new()
        {
            Id = Guid.NewGuid(),
            Email = "Test@Test.com"
        };
        // Act
        Card model = new(data, customer);
        // Assert
        Assert.Equal(data, model.CardData);
        Assert.Equal(1024, model.CardData.Length);
        Assert.Equal(customer, model.Customer);
    }
}