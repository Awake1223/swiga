namespace Swiga.API.Contracts
{
    public record RegistrationResponse(
        Guid UserId,
        string Email,
        string FirstName,    // ✅ Добавить
        string LastName,     // ✅ Добавить  
        string Role,
        string Message,
        DateTime RegisteredAt,
        Guid? RentalPointId = null);
}
