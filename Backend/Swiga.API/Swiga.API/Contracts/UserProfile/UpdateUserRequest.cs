namespace Swiga.API.Contracts.UserProfile
{
    public record UpdateUserRequest(
        string FirstName,
        string LastName,
        string PhoneNumber,
        string Email);
}
