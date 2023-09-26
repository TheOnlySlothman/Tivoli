namespace Tivoli.Models.DTO;

public class CardDto : IEntity
{
    public Guid Id { get; set; }

    public string CardData { get; set; }

    public Guid? CustomerId { get; set; }

    public CardDto(string cardData, Guid? customerId = null)
    {
        CardData = cardData;
        CustomerId = customerId;
    }

    public CardDto()
    {
        
    }
}