namespace Swiga.API.Contracts.UserProfile
{
    public record ChangePasswordRequest(string CurrentPassword, string NewPassword, string ConfirmPassword);
}
