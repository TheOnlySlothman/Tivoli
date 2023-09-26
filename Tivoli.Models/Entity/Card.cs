namespace Tivoli.Models.Entity;

/// <summary>
///    Card entity.
/// </summary>
public class Card : IEntity
{
    /// <summary>
    ///  Default constructor.
    /// </summary>
    public Card()
    {
    }

    /// <summary>
    ///   Constructor.
    /// </summary>
    /// <param name="cardData">Data on card. Max length is 1024.</param>
    /// <param name="customer">The customer assigned to card.</param>
    public Card(string cardData = "", Customer? customer = null)
    {
        CardData = cardData;
        Customer = customer;
    }

    /// <inheritdoc />
    public Guid Id { get; set; }
    
    /// <summary>
    ///     Id of the customer that owns the card.
    /// </summary>
    public Guid? CustomerId { get; set; }
    
    /// <summary>
    ///   The customer that owns the card.
    /// </summary>
    public Customer? Customer { get; set; }

    /// <summary>
    ///    The card data.
    /// </summary>
    public string CardData { get; set; } = "";
    
    public byte this[int x, int y, int z] => (byte)CardData[x * 64 + y * 16 + z];
}