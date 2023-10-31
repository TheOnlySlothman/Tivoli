using System.ComponentModel.DataAnnotations;
using Tivoli.Dal.Entities;

namespace Tivoli.BLL.DTO;

/// <summary>
///     Data transfer object for <see cref="Card"/> entity.
/// </summary>
public class CardDto : IEntity
{
    /// <summary>
    ///    Gets or sets the id of the card.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     Gets or sets the card data.
    /// </summary>
    [Required]
    public string? CardData { get; set; }

    /// <summary>
    ///     Gets or sets the customer id.
    /// </summary>
    public Guid? CustomerId { get; set; }
    
    public decimal Balance { get; set; }

    /// <summary>
    ///    Constructor for <see cref="CardDto"/> class.
    /// </summary>
    /// <param name="cardData">Data on card</param>
    /// <param name="customerId">Id of </param>
    public CardDto(string cardData, Guid? customerId = null)
    {
        CardData = cardData;
        CustomerId = customerId;
    }

    /// <summary>
    ///   Constructor for <see cref="CardDto"/> class.
    /// </summary>
    public CardDto()
    {
    }
}