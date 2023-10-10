using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Tivoli.AdminApi;
using Tivoli.BLL.DTO;
using Tivoli.Dal.Entities;
using Tivoli.Dal.Repo;
using Xunit.Abstractions;

namespace Tivoli.AdminTests.Integration.ApiControllers;

public class CardControllerTests : BaseCrudControllerTests<Card, CardDto>
{
    public CardControllerTests(CustomWebApplicationFactory<Program> factory,
        ITestOutputHelper testOutputHelper) : base(
        factory, testOutputHelper)
    {
    }


    protected override Card ConstructModel()
    {
        byte[] cardBytes = new byte[16 * 4 * 16];
        new Random().NextBytes(cardBytes);
        string cardData = Convert.ToBase64String(cardBytes);
        return new Card(cardData);
    }

    protected override CardDto ConstructDto()
    {
        byte[] cardBytes = new byte[16 * 4 * 16];
        new Random().NextBytes(cardBytes);
        string cardData = Convert.ToBase64String(cardBytes);
        return new CardDto(cardData);
    }

    protected override string ControllerName => "Card";
    protected override BaseRepo<Card> Repo => UnitOfWork.Cards;

    [Fact]
    public async void Post_Card_ReturnsOkWithCard()
    {
        try
        {
            // Arrange
            CardDto card = ConstructDto();

            const string method = "create";
            // Act
            HttpResponseMessage response = await Client.PostAsync(CombineUrl(ControllerName, method),
                new StringContent(JsonSerializer.Serialize(card), Encoding.UTF8, "application/json"));

            if (response.StatusCode == HttpStatusCode.InternalServerError)
                TestOutputHelper.WriteLine(await response.Content.ReadAsStringAsync());

            CardDto? responseValue = await response.Content.ReadFromJsonAsync<CardDto>();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(card.CardData, responseValue?.CardData);
            Assert.Equal(card.CustomerId, responseValue?.CustomerId);
        }
        finally
        {
            // Cleanup
            await Context.Database.EnsureDeletedAsync();
            await Context.Database.EnsureCreatedAsync();
        }
    }

    [Fact]
    public async void Post_Card_ReturnsBadRequest()
    {
        try
        {
            // Arrange
            CardDto card = new();

            const string method = "create";
            // Act
            HttpResponseMessage response = await Client.PostAsync(CombineUrl(ControllerName, method),
                new StringContent(JsonSerializer.Serialize(card), Encoding.UTF8, "application/json"));

            if (response.StatusCode == HttpStatusCode.InternalServerError)
                TestOutputHelper.WriteLine(await response.Content.ReadAsStringAsync());

            CardDto? responseValue = await response.Content.ReadFromJsonAsync<CardDto>();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(card.CardData, responseValue?.CardData);
            Assert.Equal(card.CustomerId, responseValue?.CustomerId);
        }
        finally
        {
            // Cleanup
            await Context.Database.EnsureDeletedAsync();
            await Context.Database.EnsureCreatedAsync();
        }
    }

    [Fact]
    public async void Get_Card_ReturnsOkWithCard()
    {
        try
        {
            // Arrange
            Card card = ConstructModel();
            Repo.Add(card);
            UnitOfWork.SaveChanges();

            string method = card.Id.ToString();
            // Act
            HttpResponseMessage response = await Client.GetAsync(CombineUrl(ControllerName, method));

            if (response.StatusCode == HttpStatusCode.InternalServerError)
                TestOutputHelper.WriteLine(await response.Content.ReadAsStringAsync());

            CardDto? responseValue = await response.Content.ReadFromJsonAsync<CardDto>();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(card.CardData, responseValue?.CardData);
            Assert.Equal(card.CustomerId, responseValue?.CustomerId);
        }
        finally
        {
            // Cleanup
            await Context.Database.EnsureDeletedAsync();
            await Context.Database.EnsureCreatedAsync();
        }
    }

    [Fact]
    public async void Get_Card_ReturnsNotFoundWithId()
    {
        // Arrange
        Guid id = Guid.NewGuid();

        string method = id.ToString();
        // Act
        HttpResponseMessage response = await Client.GetAsync(CombineUrl(ControllerName, method));

        if (response.StatusCode == HttpStatusCode.InternalServerError)
            TestOutputHelper.WriteLine(await response.Content.ReadAsStringAsync());

        Guid? responseValue = await response.Content.ReadFromJsonAsync<Guid>();

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal(id, responseValue);
    }

    [Fact]
    public async void Get_AllCards_ReturnsOkWithCards()
    {
        try
        {
            // Arrange
            List<Card> cards = new byte[10].Select(_ => ConstructModel()).ToList();
            foreach (Card card in cards) Repo.Add(card);
            UnitOfWork.SaveChanges();

            const string method = "";

            // Act
            HttpResponseMessage response = await Client.GetAsync(CombineUrl(ControllerName, method));

            if (response.StatusCode == HttpStatusCode.InternalServerError)
                TestOutputHelper.WriteLine(await response.Content.ReadAsStringAsync());

            List<CardDto>? responseValue = await response.Content
                .ReadFromJsonAsync<IEnumerable<CardDto>>() as List<CardDto>;


            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(responseValue);
            Assert.Equal(cards.Count, responseValue.Count);
            Assert.All(responseValue, dto =>
                Assert.Contains(cards, card => card.CardData == dto.CardData && card.CustomerId == dto.CustomerId));
        }
        finally
        {
            // Cleanup
            await Context.Database.EnsureDeletedAsync();
            await Context.Database.EnsureCreatedAsync();
        }
    }

    [Fact]
    public async void Put_Card_ReturnsOkWithCard()
    {
        try
        {
            // Arrange
            Card card = ConstructModel();
            Repo.Add(card);
            UnitOfWork.SaveChanges();

            CardDto cardDto = ConstructDto();
            cardDto.Id = card.Id;

            string method = card.Id.ToString();
            // Act
            HttpResponseMessage response = await Client.PutAsync(CombineUrl(ControllerName, method),
                new StringContent(JsonSerializer.Serialize(cardDto), Encoding.UTF8, "application/json"));

            if (response.StatusCode == HttpStatusCode.InternalServerError)
                TestOutputHelper.WriteLine(await response.Content.ReadAsStringAsync());

            CardDto? responseValue = await response.Content.ReadFromJsonAsync<CardDto>();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(cardDto.CardData, responseValue?.CardData);
            Assert.Equal(cardDto.CustomerId, responseValue?.CustomerId);
        }
        finally
        {
            // Cleanup
            await Context.Database.EnsureDeletedAsync();
            await Context.Database.EnsureCreatedAsync();
        }
    }

    [Fact]
    public async void Delete_Card_ReturnsOk()
    {
        try
        {
            // Arrange
            Card card = ConstructModel();
            Repo.Add(card);
            UnitOfWork.SaveChanges();

            string method = card.Id.ToString();
            // Act
            HttpResponseMessage response = await Client.DeleteAsync(CombineUrl(ControllerName, method));

            if (response.StatusCode == HttpStatusCode.InternalServerError)
                TestOutputHelper.WriteLine(await response.Content.ReadAsStringAsync());

            // Assert
            response.EnsureSuccessStatusCode();
        }
        finally
        {
            // Cleanup
            await Context.Database.EnsureDeletedAsync();
            await Context.Database.EnsureCreatedAsync();
        }
    }
}