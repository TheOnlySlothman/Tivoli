using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Tivoli.Models.DTO;
using Tivoli.Models.Entity;
using Xunit;
using Xunit.Abstractions;

namespace Tivoli.AdminTests.Integration.ApiControllers;

public class CardControllerTests : BaseCrudControllerTests<Card>
{
    public CardControllerTests(CustomWebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper) : base(
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
    
    [Fact]
    public async void Post_Card_ReturnsOkWithCard()
    {
        // Arrange
        CardDto card = ConstructDto();
        
        const string method = "create";
        // Act
        HttpResponseMessage response = await Client.PostAsync(CombineUrl(ControllerName, method), new StringContent(JsonSerializer.Serialize(card), Encoding.UTF8, "application/json"));
        
        if (response.StatusCode == HttpStatusCode.InternalServerError)
            TestOutputHelper.WriteLine(await response.Content.ReadAsStringAsync());

        CardDto? responseValue = await response.Content.ReadFromJsonAsync<CardDto>();
        
        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(card.CardData, responseValue?.CardData);
        Assert.Equal(card.CustomerId, responseValue?.CustomerId);
        
        // Cleanup
        
    }

    [Fact]
    public async void Get_Card_ReturnsOkWithCard()
    {
        // Arrange
        Card card = ConstructModel();
        UnitOfWork.Cards.Add(card);
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

    [Fact]
    public async void Get_AllCards_ReturnsOkWithCards()
    {
        // Arrange
        List<Card> cards = new byte[10].Select(_ => ConstructModel()).ToList();
        foreach (Card card in cards) UnitOfWork.Cards.Add(card);
        UnitOfWork.SaveChanges();

        const string method = "";
        
        // Act
        HttpResponseMessage response = await Client.GetAsync(CombineUrl(ControllerName, method));

        if (response.StatusCode == HttpStatusCode.InternalServerError)
            TestOutputHelper.WriteLine(await response.Content.ReadAsStringAsync());

        List<CardDto>? responseValue = await response.Content.ReadFromJsonAsync<IEnumerable<CardDto>>() as List<CardDto>;
        
        
        // Assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(responseValue);
        Assert.Equal(cards.Count, responseValue.Count);
        Assert.All(responseValue, dto => Assert.Contains(cards, card => card.CardData == dto.CardData && card.CustomerId == dto.CustomerId));
    }
    
    [Fact]
    public async void Put_Card_ReturnsOkWithCard()
    {
        // Arrange
        Card card = ConstructModel();
        UnitOfWork.Cards.Add(card);
        UnitOfWork.SaveChanges();
        
        CardDto cardDto = ConstructDto();
        cardDto.Id = card.Id;
        
        string method = card.Id.ToString();
        // Act
        HttpResponseMessage response = await Client.PutAsync(CombineUrl(ControllerName, method), new StringContent(JsonSerializer.Serialize(cardDto), Encoding.UTF8, "application/json"));

        if (response.StatusCode == HttpStatusCode.InternalServerError)
            TestOutputHelper.WriteLine(await response.Content.ReadAsStringAsync());

        CardDto? responseValue = await response.Content.ReadFromJsonAsync<CardDto>();
        
        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(cardDto.CardData, responseValue?.CardData);
        Assert.Equal(cardDto.CustomerId, responseValue?.CustomerId);
    }
    
}