using Swiga.Domain.Models;

namespace Swiga.API.Contracts.Registration
{
    public record RegisterClientRequest(
         string FirstName,
         string LastName ,
         DateOnly? DateOfBirth,
         string Email,
         string Password,
         string? PhoneNumber = null);
}
