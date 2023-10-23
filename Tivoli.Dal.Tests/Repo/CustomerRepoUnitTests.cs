using System.Linq.Expressions;
using Tivoli.Dal.Entities;
using Tivoli.Dal.Repo;

namespace Tivoli.Dal.Tests.Repo;

// ReSharper disable once UnusedType.Global
public class CustomerRepoUnitTests : BaseRepoUnitTests<Customer>
{
    protected override BaseRepo<Customer> Repo => UnitOfWork.Customers;

    protected override Customer CreateModel()
    {
        string data = string.Join("", Enumerable.Range(0, 1024).Select(x => (char)(byte)x));

        Card card = new(data);

        Customer customer = new()
        {
            AccessFailedCount = 0,
            ConcurrencyStamp = null,
            Email = "Test@Test.com",
            EmailConfirmed = false,
            Id = Guid.NewGuid(),
            LockoutEnabled = false,
            LockoutEnd = null,
            NormalizedEmail = null,
            NormalizedUserName = null,
            PasswordHash = null,
            PhoneNumber = null,
            PhoneNumberConfirmed = false,
            SecurityStamp = null,
            TwoFactorEnabled = false,
            UserName = "TestUser"
        };

        customer.Cards.Add(card);

        return customer;
    }

    protected override Customer UpdateModel(Customer model)
    {
        string data = string.Join("", Enumerable.Range(0, 1024).Reverse().Select(x => (char)(byte)x));

        model.Cards.First().CardData = data;

        Card card = new(string.Join("", Enumerable.Repeat((char)0, 1024)));
        model.Cards.Add(card);
        return model;
    }

    protected override Expression<Func<Customer, object?>>[] GetRelations()
    {
        return new Expression<Func<Customer, object?>>[]
        {
            x => x.Cards
        };
    }
}