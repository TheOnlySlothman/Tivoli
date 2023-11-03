using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Tivoli.BLL.DTO;
using Tivoli.Dal.Entities;
using Tivoli.Dal.Repo;
using Xunit.Abstractions;

namespace Tivoli.CustomerApi.Tests;

public class ScannerControllerTests : BaseCrudControllerTests<Card, CardDto>
{
    public ScannerControllerTests(CustomWebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper) :
        base(
            factory, testOutputHelper)
    {
    }

    protected override string ControllerName => "Scanner";
    protected override BaseRepo<Card> Repo => UnitOfWork.Cards;

    protected override Card ConstructModel()
    {
        return new Card(string.Create(1028, 'a', (span, value) => span.Fill(value)));
    }

    protected override CardDto ConstructDto()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public async void Get_ValidateAndPayWithCard_ReturnOk()
    {
        await UseDatabase(Callback);
        return;

        async Task Callback()
        {
            // Arrange
            const string method = "ValidateAndPayWithCard";
            const string token = "f2f48034-5827-4e53-9d45-ac320501b3a6";
            Card card = ConstructModel();
            card.Balance = 100;
            decimal originalBalance = card.Balance;

            Repo.Add(card);
            await UnitOfWork.SaveChangesAsync();

            HttpResponseMessage response =
                Client.GetAsync($"{ControllerName}/{method}/{token}?cardData={card.CardData}").Result;

            // Assert
            UnitOfWork = Factory.Services.CreateScope().ServiceProvider.GetRequiredService<UnitOfWork>();

            Assert.True(response.IsSuccessStatusCode);
            UnitOfWork = Factory.Services.CreateScope().ServiceProvider.GetRequiredService<UnitOfWork>();
            Assert.Equal(originalBalance - 11, Repo.Get(card.Id).Balance);
        }
    }

    [Fact]
    public async void Get_ValidateAndPayWithCardNoBalance_ReturnBadRequest()
    {
        await UseDatabase(Callback);
        return;

        async Task Callback()
        {
            // Arrange
            const string method = "ValidateAndPayWithCard";
            const string token = "f2f48034-5827-4e53-9d45-ac320501b3a6";
            Card card = ConstructModel();
            card.Balance = 0;
            decimal originalBalance = card.Balance;

            Repo.Add(card);
            await UnitOfWork.SaveChangesAsync();

            HttpResponseMessage response =
                Client.GetAsync($"{ControllerName}/{method}/{token}?cardData={card.CardData}").Result;

            // Assert
            UnitOfWork = Factory.Services.CreateScope().ServiceProvider.GetRequiredService<UnitOfWork>();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            UnitOfWork = Factory.Services.CreateScope().ServiceProvider.GetRequiredService<UnitOfWork>();
            Assert.Equal(originalBalance, Repo.Get(card.Id).Balance);
        }
    }

    [Fact]
    public async void Get_ValidateAndPayWithCardNoCard_ReturnBadRequest()
    {
        await UseDatabase(Callback);
        return;

        Task Callback()
        {
            // Arrange
            const string method = "ValidateAndPayWithCard";
            const string token = "f2f48034-5827-4e53-9d45-ac320501b3a6";
            Card card = ConstructModel();
            card.Balance = 100;

            HttpResponseMessage response =
                Client.GetAsync($"{ControllerName}/{method}/{token}?cardData={card.CardData}").Result;

            // Assert
            UnitOfWork = Factory.Services.CreateScope().ServiceProvider.GetRequiredService<UnitOfWork>();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            return Task.CompletedTask;
        }
    }
}