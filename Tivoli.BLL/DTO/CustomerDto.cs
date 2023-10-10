using System.ComponentModel.DataAnnotations;
using Tivoli.Dal.Entities;

namespace Tivoli.BLL.DTO;

/// <summary>
///     Customer DTO.
/// </summary>
public class CustomerDto : IEntity
{
    /// <inheritdoc />
    public Guid Id { get; set; }

    /// <summary>
    ///     Gets or sets the username of customer.
    /// </summary>
    [Required]
    public string UserName { get; set; } = default!;

    /// <summary>
    ///    Gets or sets the email of customer.
    /// </summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;

    /// <summary>
    ///     Gets or sets the phone number of customer.
    /// </summary>
    public string PhoneNumber { get; set; } = default!;

    /// <summary>
    ///    Constructor for <see cref="CustomerDto"/> class.
    /// </summary>
    /// <param name="userName">Username of customer.</param>
    /// <param name="email">Email of customer.</param>
    /// <param name="phoneNumber">Phone number of customer.</param>
    public CustomerDto(string userName, string email, string phoneNumber)
    {
        UserName = userName;
        Email = email;
        PhoneNumber = phoneNumber;
    }

    /// <summary>
    ///    Constructor for <see cref="CustomerDto"/> class.
    /// </summary>
    public CustomerDto()
    {
    }
}