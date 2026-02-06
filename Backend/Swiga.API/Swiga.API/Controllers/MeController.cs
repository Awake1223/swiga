using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swiga.API.Contracts.UserProfile;
using Swiga.Application.Services;
using Swiga.Application.Services.UserServices;
using Swiga.Domain.Models;

namespace Swiga.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class MeController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<MeController> _logger;

        public MeController(IUserService userService, ICurrentUserService currentUserService, ILogger<MeController> logger)
        {
            _userService = userService;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(UserProfileResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserProfileResponse>> GetMyProfile()
        {
            var userId = _currentUserService.GetCurrentUserId();
            if (!userId.HasValue)
                return Unauthorized(new { error = "Пользователь не авторизован" });

            var user = await _userService.GetUserByIdAsync(userId.Value);
            if(user == null)
            {
                _logger.LogWarning("User {UserId} not found in database", userId);
                return NotFound(new { error = "Пользователь не найден" });
            }

            var response = MapToProfileResponse(user);

            _logger.LogInformation("User {UserId} retrieved their profile", userId);

            return Ok(response);

        }

        [HttpPut("profile")]
        [ProducesResponseType(typeof(UserProfileResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserProfileResponse>> UpdateMyProfile([FromBody] UpdateProfileRequest request)
        {
            var userId = _currentUserService.GetCurrentUserId();
                if (!userId.HasValue) return Unauthorized(new { error = "Пользователь не авторизован" });

            var validationError = ValidateUpdateProfileRequest(request);
            if (!string.IsNullOrEmpty(validationError))
                return BadRequest(new { error = validationError });

            var user = await _userService.GetUserByIdAsync(userId.Value);
            if (user == null)
                return NotFound(new { error = "Пользователь не найден" });

            if(!string.Equals(user.Email, request.Email, StringComparison.OrdinalIgnoreCase))
            {
                var existingUser = await _userService.GetUserByEmailAsync(request.Email);
                if (existingUser != null && existingUser.Id != userId.Value)
                    return BadRequest(new { error = "Почта занята другим пользователем" });
            }

            user.UpdateProfile(
                request.FirstName,
                request.LastName,
                request.Email,
                request.PhoneNumber ?? string.Empty);

            await _userService.UpdateUser(user);

            _logger.LogInformation("Пользователь {UserId} обновил профиль", userId);

            return Ok(MapToProfileResponse(user));


        }

        [HttpPut("password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            var userId = _currentUserService.GetCurrentUserId();

            if (!userId.HasValue)
                return Unauthorized(new { error = "Пользователь не авторизован" });

            var validationError = ValidateChangePasswordRequest(request);

            if(!string.IsNullOrEmpty(validationError))
                return BadRequest(new {error = validationError});

            var (success, error) = await _userService.ChangePasswordAsync(userId.Value, request.CurrentPassword, request.NewPassword);

            if(!success)
                return BadRequest(error);

            _logger.LogInformation("Пользователь {UserId} поменял свой пароль", userId);

            return Ok(new { message = "Пароль изменен успешно" });

        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> DeleteMyAccount([FromBody] DeleteAccountRequest request)
        {
            var userId = _currentUserService.GetCurrentUserId();
            if (userId.HasValue)
                return Unauthorized(new { error = " Пользователь не авторизован" });

            if (string.IsNullOrWhiteSpace(request.Password))
                return BadRequest(new { error = "Пароль запрошен" });

            var (success, error) = await _userService.DeleteUserAsync(userId.Value, request.Password);

            if(!success)
                return BadRequest(new {error});

            _logger.LogInformation("User {UserId} deleted their account", userId);

            return Ok(new { message = "Аккаунт удален успешно", timestamp = DateTime.UtcNow });
        }

        private string ValidateUpdateProfileRequest(UpdateProfileRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.FirstName))
                return "FirstName is required";

            if (string.IsNullOrWhiteSpace(request.LastName))
                return "LastName is required";

            if (string.IsNullOrWhiteSpace(request.Email))
                return "Email is required";

            if (!IsValidEmail(request.Email))
                return "Email is not valid";

            if (request.PhoneNumber != null && request.PhoneNumber.Length > 20)
                return "PhoneNumber is too long";

            return string.Empty;
        }

        private string ValidateChangePasswordRequest(ChangePasswordRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.CurrentPassword))
                return "Current password is required";

            if (string.IsNullOrWhiteSpace(request.NewPassword))
                return "New password is required";

            if (request.NewPassword.Length < 6)
                return "New password must be at least 6 characters long";

            if (!string.Equals(request.NewPassword, request.ConfirmPassword, StringComparison.Ordinal))
                return "New password and confirmation do not match";

            return string.Empty;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        private UserProfileResponse MapToProfileResponse(UserModel user)
        {
            var response = new UserProfileResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role.ToString(),
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };

            if (user is AdminModel admin)
                response.RentalPointId = admin.RentalPointId;

            return response;
        }

    }
}
