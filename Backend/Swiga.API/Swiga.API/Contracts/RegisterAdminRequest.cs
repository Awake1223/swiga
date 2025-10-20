namespace Swiga.API.Contracts
{
    public record RegisterAdminRequest(
        string Email,
        string Password,
        string FirstName,  // ✅ Для будущего рефакторинга
        string LastName,   // ✅
        string? PhoneNumber = null,
        Guid? RentalPointId = null,
        bool CreateNewRentalPoint = false,
        string? RentalPointName = null,
        string? RentalPointAddress = null,
        string? RentalPointCity = null
    );
}
