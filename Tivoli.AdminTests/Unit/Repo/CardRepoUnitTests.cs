using System.Linq.Expressions;
using Tivoli.Dal.Repo;
using Tivoli.Models.Entity;

namespace Tivoli.AdminTests.Unit.Repo;

// ReSharper disable once UnusedType.Global
public class CardRepoUnitTests : BaseRepoUnitTests<Card>
{
    protected override BaseRepo<Card> Repo => UnitOfWork.Cards;
    protected override Card CreateModel()
    {
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
        
        return new Card(data.ToString(), customer);
    }

    protected override Card UpdateModel(Card model)
    {
        model.Customer = new Customer
        {
            Id = new Guid(),
            Email = "Test2@Test.com"
        };
        return model;
    }

    protected override Expression<Func<Card, object?>>[] GetRelations()
    {
        return new Expression<Func<Card, object?>>[]
        {
            x => x.Customer
        };
    }
}