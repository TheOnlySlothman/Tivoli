namespace Tivoli.Dal.Entities;

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
    ///     Gets or sets id of the customer that owns the card.
    /// </summary>
    public Guid? CustomerId { get; set; }

    /// <summary>
    ///   Gets or sets the customer that owns the card.
    /// </summary>
    public Customer? Customer { get; set; }

    /// <summary>
    ///    Gets or sets the card data.
    /// </summary>
    public string CardData { get; set; } = "";

    /// <summary>
    ///   The card data as a byte array.
    /// </summary>
    /// <param name="sector">Sector of card.</param>
    /// <param name="block">Block of card.</param>
    /// <param name="byte">Byte of card.</param>
    public byte this[int sector, int block, int @byte] => (byte)CardData[sector * 64 + block * 16 + @byte];

    public decimal Balance { get; set; } = 0;
}