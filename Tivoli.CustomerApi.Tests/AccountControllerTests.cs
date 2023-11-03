using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tivoli.BLL.DTO;
using Tivoli.BLL.Models;
using Tivoli.Dal;
using Tivoli.Dal.Entities;
using Tivoli.Dal.Repo;
using Xunit.Abstractions;

namespace Tivoli.CustomerApi.Tests;

public class AccountControllerTests : BaseCrudControllerTests<Card, CardDto>
{
    public AccountControllerTests(CustomWebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper) : base(
        factory, testOutputHelper)
    {
    }

    protected override string ControllerName => "Account";
    protected override BaseRepo<Card> Repo => UnitOfWork.Cards;

    protected override Card ConstructModel()
    {
        Card card = new()
        {
            Balance = 100
        };
        return card;
    }

    protected override CardDto ConstructDto()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public async Task Get_Card_ReturnOk()
    {
        await UseDatabase(Callback);
        return;

        async Task Callback()
        {
            // Arrange
            const string method = "Cards";
            string username = "Test@Test.dk";
            string password = "Test123.";
            Customer customer = await AddUserAsync(username, password);
            List<Card> cards = new byte[10].Select(_ => ConstructModel()).ToList();
            foreach (Card card in cards)
            {
                card.CustomerId = customer.Id;
                Repo.Add(card);
            }

            await UnitOfWork.SaveChangesAsync();

            await Login();
            // Act
            HttpResponseMessage response = await Client.GetAsync(CombineUrl(ControllerName, method));

            if (response.StatusCode == HttpStatusCode.InternalServerError)
                TestOutputHelper.WriteLine(await response.Content.ReadAsStringAsync());

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            List<CardDto>? result = await response.Content.ReadFromJsonAsync<List<CardDto>>();
            Assert.NotNull(result);
            Assert.Equal(cards.Count, result.Count);
        }
    }

    // [Fact]
    // public async Task Post_ReturnOk()
    // {
    //     await UseDatabase(Callback);
    //     return;
    //
    //     async Task Callback()
    //     {
    //         // Arrange
    //         const string method = "Cards";
    //         string username = "Test@Test.dk";
    //         string password = "Test123.";
    //
    //         Customer customer = await AddUserAsync(username, password);
    //         Card card = ConstructModel();
    //         card.Customer = customer;
    //         Repo.Add(card);
    //         await UnitOfWork.SaveChangesAsync();
    //
    //         await Login();
    //
    //         // Act
    //         HttpResponseMessage response = await Client.PostAsync(CombineUrl(ControllerName, method),
    //             new StringContent(JsonSerializer.Serialize(new CardDto(card.CardData, card.CustomerId)), Encoding.UTF8,
    //                 "application/json"));
    //
    //         if (response.StatusCode == HttpStatusCode.InternalServerError)
    //             TestOutputHelper.WriteLine(await response.Content.ReadAsStringAsync());
    //
    //         // Assert
    //         Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    //         Assert.True(Repo.Exists(card.Id));
    //     }
    // }

    [Fact]
    public async Task Put_AddBalance_ReturnOk()
    {
        await UseDatabase(Callback);
        return;

        async Task Callback()
        {
            // Arrange
            const string method = "AddBalance";
            string username = "Test@Test.dk";
            string password = "Test123.";

            Customer customer = await AddUserAsync(username, password);
            Card card = ConstructModel();
            card.CustomerId = customer.Id;
            Repo.Add(card);
            await UnitOfWork.SaveChangesAsync();

            await Login();

            AddBalanceDto addBalanceDto = new()
            {
                CardId = card.Id,
                Amount = card.Balance
            };

            // Act
            HttpResponseMessage response = await Client.PutAsync(CombineUrl(ControllerName, method),
                new StringContent(JsonSerializer.Serialize(addBalanceDto), Encoding.UTF8,
                    "application/json"));

            if (response.StatusCode == HttpStatusCode.InternalServerError)
                TestOutputHelper.WriteLine(await response.Content.ReadAsStringAsync());

            UnitOfWork = Factory.Services.CreateScope().ServiceProvider.GetRequiredService<UnitOfWork>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(Repo.Get(card.Id).Balance == 200);
        }
    }


    private async Task Login(string username = "Test@Test.dk", string password = "Test123.")
    {
        HttpResponseMessage result = await Client.PostAsync(CombineUrl("Auth", "Login"),
            new StringContent(JsonSerializer.Serialize(new LoginDto(username, password)), Encoding.UTF8,
                "application/json"));

        TokenRequest? tokenRequest = result.Content.ReadFromJsonAsync<TokenRequest>().Result;
        if (tokenRequest is not null)
        {
            Client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer",
                    tokenRequest.Token);
        }
        else throw new InvalidOperationException();
    }
}