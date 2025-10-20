using Microsoft.AspNetCore.Mvc;
using Swiga.Domain.Models;
using Swiga.API.Contracts;
using Swiga.Application.Services;
using Microsoft.AspNetCore.Identity.Data;

namespace Swiga.API.Controllers
{
    [Controller]
    [Route("[controller]")]
    public class RegistrationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRentalPointService _rentalPointService;

        public RegistrationController(
            IUserService userService,
            IRentalPointService rentalPointService)
        {
            _userService = userService;
            _rentalPointService = rentalPointService;
        }

        [HttpPost("client")]
        public async Task<ActionResult<RegistrationResponse>> RegisterClient([FromBody] RegisterClientRequest request)
        {
            var client = ClientModel.Create(
                request.FirstName,
                request.LastName,
                request.Email,
                request.PhoneNumber ?? string.Empty,
                request.Password
                );

            var userId = await _userService.CreateUser(client);

            return Ok(new { UserId = userId, Message = "Registration successful" });

        }

        [HttpPost("admin")]
        public async Task<ActionResult<RegistrationResponse>> RegisterAdmin([FromBody] RegisterAdminRequest request)
        {
           // string fullName = $"{request.FirstName} {request.LastName}";
            var admin = AdminModel.Create(
                request.FirstName,
                request.LastName,
                request.RentalPointId ?? Guid.Empty,
                request.Email,
                request.PhoneNumber ?? string.Empty,
                request.Password);

            var userId = await _userService.CreateUser(admin);

            return Ok(new { UserId = userId, Message = "Registration successful" });
        }

    }
}
