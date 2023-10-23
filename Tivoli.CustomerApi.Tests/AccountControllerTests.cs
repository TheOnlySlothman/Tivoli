using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tivoli.BLL.Models;
using Tivoli.Dal.Repo;
using Xunit.Abstractions;

namespace Tivoli.CustomerApi.Tests;

public class AccountControllerTests : BaseControllerTests
{
    public AccountControllerTests(CustomWebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper) :
        base(
            factory, testOutputHelper)
    {
    }

    private static string ControllerName = "Account";

    [Fact]
    public void Register_ReturnAccepted()
    {
        UseDatabase(Callback);
        return;

        async void Callback()
        {
            // Arrange
            RegisterDto card = new() { Username = "Test@Test.dk", Password = "Test123." };

            HttpContent content = new StringContent(JsonSerializer.Serialize(card), Encoding.UTF8, "application/json");

            string method = "Register";
            // Act
            HttpResponseMessage response = await Client.PostAsync(CombineUrl(ControllerName, method), content);

            if (response.StatusCode == HttpStatusCode.InternalServerError)
                TestOutputHelper.WriteLine(await response.Content.ReadAsStringAsync());


            // Assert
            response.EnsureSuccessStatusCode();
        }
    }

    protected void UseDatabase(Action callback)
    {
        try
        {
            callback();
        }
        finally
        {
            // Cleanup
            // Context.Database.EnsureDeleted();
            using IServiceScope scope = Factory.Services.CreateScope();
            DbContext db = scope.ServiceProvider.GetRequiredService<DbContext>();
            db.Database.EnsureDeleted();
        }
    }
}