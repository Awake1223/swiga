namespace Swiga.API.Contracts
{
    public record UpdateUserRequest(
        string FirstName,
        string LastName,
        string PhoneNumber,
        string Email);
}
