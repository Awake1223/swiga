namespace Swiga.API.Contracts.Registration
{
    public record RegisterAdminRequest(
        string Email,
        string Password,
        string FirstName,  // ✅ Для будущего рефакторинга
        string LastName,   // ✅
        string? PhoneNumber = null,
        string? RentalPointId = null,
        bool CreateNewRentalPoint = false,
        string? RentalPointName = null,
        string? RentalPointAddress = null,
        string? RentalPointCity = null
    );
}
