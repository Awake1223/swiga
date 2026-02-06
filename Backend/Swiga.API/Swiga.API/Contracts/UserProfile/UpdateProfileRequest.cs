namespace Swiga.API.Contracts.UserProfile
{
    public record UpdateProfileRequest(string FirstName, string LastName, string Email, string? PhoneNumber);
}
