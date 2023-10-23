using System.Linq.Expressions;
using Tivoli.Dal.Entities;
using Tivoli.Dal.Repo;

namespace Tivoli.Dal.Tests.Repo;

// ReSharper disable once UnusedType.Global
public class CardRepoUnitTests : BaseRepoUnitTests<Card>
{
    protected override BaseRepo<Card> Repo => UnitOfWork.Cards;

    protected override Card CreateModel()
    {
        string data = string.Join("", Enumerable.Range(0, 1024).Select(x => (char)(byte)x));
        Customer customer = new()
        {
            Id = new Guid(),
            Email = "Test@Test.com"
        };

        return new Card(data, customer);
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