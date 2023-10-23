using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Tivoli.BLL.DTO;
using Tivoli.Dal.Entities;
using Tivoli.Dal.Repo;
using Xunit.Abstractions;

namespace Tivoli.AdminApi.Tests;

public class CustomerControllerTests : BaseCrudControllerTests<Customer, CustomerDto>
{
    public CustomerControllerTests(CustomWebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper) :
        base(factory, testOutputHelper)
    {
    }

    protected override string ControllerName => "Customer";
    protected override BaseRepo<Customer> Repo => UnitOfWork.Customers;

    protected override Customer ConstructModel()
    {
        return new Customer();
    }

    protected override CustomerDto ConstructDto()
    {
        return new CustomerDto("TestUser", "TestEmail@Test.dk", "88888888");
    }

    [Fact]
    public async void Post_Customer_ReturnsOkWithCustomer()
    {
        try
        {
            // Arrange
            CustomerDto customerDto = ConstructDto();

            const string method = "create";
            // Act
            HttpResponseMessage response = await Client.PostAsync(CombineUrl(ControllerName, method),
                new StringContent(JsonSerializer.Serialize(customerDto), Encoding.UTF8, "application/json"));

            if (response.StatusCode == HttpStatusCode.InternalServerError)
                TestOutputHelper.WriteLine(await response.Content.ReadAsStringAsync());

            CustomerDto? responseValue = await response.Content.ReadFromJsonAsync<CustomerDto>();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(customerDto.UserName, responseValue?.UserName);
            Assert.Equal(customerDto.Email, responseValue?.Email);
            Assert.Equal(customerDto.PhoneNumber, responseValue?.PhoneNumber);
        }
        finally
        {
            // Cleanup
            await Context.Database.EnsureDeletedAsync();
            await Context.Database.EnsureCreatedAsync();
        }
    }

    [Fact]
    public async void Post_Customer_ReturnsBadRequest()
    {
        try
        {
            // Arrange
            CustomerDto customerDto = new();

            const string method = "create";
            // Act
            HttpResponseMessage response = await Client.PostAsync(CombineUrl(ControllerName, method),
                new StringContent(JsonSerializer.Serialize(customerDto), Encoding.UTF8, "application/json"));

            if (response.StatusCode == HttpStatusCode.InternalServerError)
                TestOutputHelper.WriteLine(await response.Content.ReadAsStringAsync());

            CustomerDto? responseValue = await response.Content.ReadFromJsonAsync<CustomerDto>();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(customerDto.UserName, responseValue?.UserName);
            Assert.Equal(customerDto.Email, responseValue?.Email);
            Assert.Equal(customerDto.PhoneNumber, responseValue?.PhoneNumber);
        }
        finally
        {
            // Cleanup
            await Context.Database.EnsureDeletedAsync();
            await Context.Database.EnsureCreatedAsync();
        }
    }

    [Fact]
    public async void Get_Customer_ReturnsOkWithCustomer()
    {
        try
        {
            // Arrange
            Customer customer = ConstructModel();

            Repo.Add(customer);
            UnitOfWork.SaveChanges();

            string method = customer.Id.ToString();
            // Act
            HttpResponseMessage response = await Client.GetAsync(CombineUrl(ControllerName, method));

            if (response.StatusCode == HttpStatusCode.InternalServerError)
                TestOutputHelper.WriteLine(await response.Content.ReadAsStringAsync());

            CustomerDto? responseValue = await response.Content.ReadFromJsonAsync<CustomerDto>();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(customer.UserName, responseValue?.UserName);
            Assert.Equal(customer.Email, responseValue?.Email);
            Assert.Equal(customer.PhoneNumber, responseValue?.PhoneNumber);
        }
        finally
        {
            // Cleanup
            await Context.Database.EnsureDeletedAsync();
            await Context.Database.EnsureCreatedAsync();
        }
    }

    [Fact]
    public async void Get_Customer_ReturnsNotFoundWithId()
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
    public async void Get_AllCustomers_ReturnsOkWithCustomers()
    {
        try
        {
            // Arrange
            List<Customer> customers = new byte[10].Select(_ => ConstructModel()).ToList();
            foreach (Customer customer in customers) Repo.Add(customer);
            UnitOfWork.SaveChanges();

            const string method = "";

            // Act
            HttpResponseMessage response = await Client.GetAsync(CombineUrl(ControllerName, method));

            if (response.StatusCode == HttpStatusCode.InternalServerError)
                TestOutputHelper.WriteLine(await response.Content.ReadAsStringAsync());

            List<CustomerDto>? responseValue = await response.Content
                .ReadFromJsonAsync<IEnumerable<CustomerDto>>() as List<CustomerDto>;


            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(responseValue);
            Assert.Equal(customers.Count, responseValue.Count);
            Assert.All(responseValue, dto =>
                Assert.Contains(customers, customer => customer.UserName == dto.UserName
                                                       && customer.Email == dto.Email
                                                       && customer.PhoneNumber == dto.PhoneNumber));
        }
        finally
        {
            // Cleanup
            await Context.Database.EnsureDeletedAsync();
            await Context.Database.EnsureCreatedAsync();
        }
    }

    [Fact]
    public async void Put_Customer_ReturnsOkWithCustomer()
    {
        try
        {
            // Arrange
            Customer customer = ConstructModel();
            Repo.Add(customer);
            UnitOfWork.SaveChanges();

            CustomerDto customerDto = ConstructDto();
            customerDto.Id = customer.Id;

            string method = customer.Id.ToString();
            // Act
            HttpResponseMessage response = await Client.PutAsync(CombineUrl(ControllerName, method),
                new StringContent(JsonSerializer.Serialize(customerDto), Encoding.UTF8, "application/json"));

            if (response.StatusCode == HttpStatusCode.InternalServerError)
                TestOutputHelper.WriteLine(await response.Content.ReadAsStringAsync());

            CustomerDto? responseValue = await response.Content.ReadFromJsonAsync<CustomerDto>();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(customer.UserName, responseValue?.UserName);
            Assert.Equal(customer.Email, responseValue?.Email);
            Assert.Equal(customer.PhoneNumber, responseValue?.PhoneNumber);
        }

        finally
        {
            // Cleanup
            await Context.Database.EnsureDeletedAsync();
            await Context.Database.EnsureCreatedAsync();
        }
    }

    [Fact]
    public async void Delete_Customer_ReturnsOk()
    {
        try
        {
            // Arrange
            Customer customer = ConstructModel();
            Repo.Add(customer);
            UnitOfWork.SaveChanges();

            string method = customer.Id.ToString();
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