using System.Text.Json;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.IdentityModel.Tokens;
using Swiga.API.Contracts;
using Swiga.Application.Services;
using Swiga.Domain.Models;

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

            var response = new RegistrationResponse(
                UserId: userId,
                Email: client.Email,
                FirstName: client.FirstName,
                LastName: client.LastName,
                Role: "Client",
                Message: "Client registration successful. Welcome to Swiga!",
                RegisteredAt: DateTime.UtcNow
            );

            return Ok(response);

        }
        [HttpPost("admin")]
        public async Task<ActionResult<RegistrationResponse>> RegisterAdmin([FromBody] RegisterAdminRequest request)
        {
            try
            {
                Console.WriteLine($"Received request: {JsonSerializer.Serialize(request)}");

                if (request == null)
                {
                    Console.WriteLine("Request is null");
                    return BadRequest("Request body cannot be null");
                }

                Console.WriteLine($"FirstName: '{request.FirstName}'");
                Console.WriteLine($"LastName: '{request.LastName}'");
                Console.WriteLine($"Email: '{request.Email}'");
                Console.WriteLine($"Password: '{request.Password}'");
                Console.WriteLine($"RentalPointId: '{request.RentalPointId}'");

                if (string.IsNullOrEmpty(request.FirstName))
                    return BadRequest("FirstName is required");

                if (string.IsNullOrEmpty(request.LastName))
                    return BadRequest("LastName is required");

                if (string.IsNullOrEmpty(request.Email))
                    return BadRequest("Email is required");

                if (string.IsNullOrEmpty(request.Password))
                    return BadRequest("Password is required");

                Guid rentalPointId = Guid.Empty;
                if (!string.IsNullOrEmpty(request.RentalPointId))
                {
                    if (Guid.TryParse(request.RentalPointId, out Guid parsedId))
                    {
                        rentalPointId = parsedId;
                    }
                    else
                    {
                        return BadRequest($"Invalid RentalPointId format: '{request.RentalPointId}'. Expected valid GUID.");
                    }
                }

                var admin = AdminModel.Create(
                    request.FirstName,
                    request.LastName,
                    rentalPointId,
                    request.Email,
                    request.PhoneNumber ?? string.Empty,
                    request.Password);

                var userId = await _userService.CreateUser(admin);

                var response = new RegistrationResponse(
                    UserId: userId,
                    Email: admin.Email,
                    FirstName: admin.FirstName,
                    LastName: admin.LastName,
                    Role: "Admin",
                    Message: "Admin registration successful. Welcome to Swiga!",
                    RegisteredAt: DateTime.UtcNow
                );

                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Registration error: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, new { error = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpPut("{id:guid}")] 
        public async Task<ActionResult> UpdateUser(Guid id,[FromBody] UpdateUserRequest request)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;

            await _userService.UpdateUser(user);

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteUser(Guid id)
        {
            await _userService.DeleteUser(id);
            return NoContent();
        }

    }
}
