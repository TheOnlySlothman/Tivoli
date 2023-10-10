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
        return _dbSet.Where(c => c.CustomerId == customerId).ToList();
    }
}