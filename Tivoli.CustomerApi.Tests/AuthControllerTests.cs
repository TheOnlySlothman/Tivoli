using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Tivoli.BLL.DTO;
using Tivoli.BLL.Models;
using Tivoli.Dal.Entities;
using Xunit.Abstractions;

namespace Tivoli.CustomerApi.Tests;

public class AuthControllerTests : BaseControllerTests
{
    public AuthControllerTests(CustomWebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper) :
        base(
            factory, testOutputHelper)
    {
    }

    private const string ControllerName = "Auth";

    [Fact]
    public async Task Register_ReturnAccepted()
    {
        await UseDatabase(Callback);
        return;

        async Task Callback()
        {
            // Arrange
            RegisterDto registerDto = new() { Username = "Test@Test.dk", Password = "Test123." };

            HttpContent content =
                new StringContent(JsonSerializer.Serialize(registerDto), Encoding.UTF8, "application/json");

            const string method = "Register";
            // Act
            HttpResponseMessage response = await Client.PostAsync(CombineUrl(ControllerName, method), content);

            if (response.StatusCode == HttpStatusCode.InternalServerError)
                TestOutputHelper.WriteLine(await response.Content.ReadAsStringAsync());


            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
            Assert.Equal(1, DbContext.Users.Count());
        }
    }

    [Fact]
    public async Task Login_ReturnAccepted()
    {
        await UseDatabase(Callback);
        return;

        async Task Callback()
        {
            // Arrange
            const string username = "Test@Test.dk";
            const string password = "Test123.";
            await AddUserAsync(username, password);

            LoginDto loginDto = new(username, password);
            // Act
            HttpResponseMessage response = await Client.PostAsync(CombineUrl(ControllerName, "Login"),
                new StringContent(JsonSerializer.Serialize(loginDto), Encoding.UTF8, "application/json"));

            if (response.StatusCode == HttpStatusCode.InternalServerError)
                TestOutputHelper.WriteLine(await response.Content.ReadAsStringAsync());

            TokenRequest? tokenRequest = await response.Content.ReadFromJsonAsync<TokenRequest>();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
            Assert.NotNull(tokenRequest);
            Assert.NotNull(tokenRequest.Token);
        }
    }
}