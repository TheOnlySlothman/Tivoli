using Microsoft.EntityFrameworkCore;
using Tivoli.Dal.Entities;

namespace Tivoli.Dal.Repo;

public class CardRepo : BaseRepo<Card>
{
    public CardRepo(DbContext context) : base(context)
    {
    }

    public IEnumerable<Card> GetCardsByCustomerId(Guid customerId)
    {
        return DbSet.Where(c => c.CustomerId == customerId).ToList();
    }

    public bool Exists(string cardData)
    {
        return DbSet.Any(c => c.CardData == cardData);
    }

    public Card GetByCardData(string cardData)
    {
        return DbSet.First(c => c.CardData == cardData);
    }
}