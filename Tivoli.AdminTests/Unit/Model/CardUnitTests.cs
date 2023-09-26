using Tivoli.Models.Entity;
using Xunit;
using Assert = Xunit.Assert;

namespace Tivoli.AdminTests.Unit.Model;

public class CardUnitTests
{
    [Fact]
    public void ConstructCardWithArray()
    {
        // Arrange
        byte[,,] data = new byte[16, 4, 16];
        
        for (byte i = 0; i < 16; i++)
        for (byte j = 0; j < 4; j++)
        for (byte k = 0; k < 16; k++)
            data[i, j, k] = (byte)(j * 16 + k);
        
        Customer customer = new()
        {
            Id = new Guid(),
            Email = "Test@Test.com"
        };
        // Act
        Card model = new(data.ToString(), customer);
        // Assert
        Assert.Equal(data.ToString(), model.CardData);
        Assert.Equal(customer, model.Customer);
    }
}