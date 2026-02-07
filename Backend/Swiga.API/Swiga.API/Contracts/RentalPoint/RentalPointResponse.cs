namespace Swiga.API.Contracts.RentalPoint
{
    public record RentalPointResponse(
     Guid Id,
     string Name, 
     string Address, 
     string City, 
     string PhoneNumber,  
     string Email);
}
