namespace Swiga.API.Contracts
{
    public record RentalPointResponse(
     Guid Id,
     string Name, 
     string Address, 
     string City, 
     string PhoneNumber,  
     string Email);
}
