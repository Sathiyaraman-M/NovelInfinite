using System.Text;
using System.Text.Encodings.Web;
using Infinite.Core.Interfaces.Services;
using Infinite.Core.Interfaces.Services.Identity;
using Infinite.Shared.Constants;
using Infinite.Shared.Entities;
using Infinite.Shared.Requests;
using Infinite.Shared.Requests.Identity;
using Infinite.Shared.Responses.Identity;
using Infinite.Shared.Wrapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;

namespace Infinite.Core.Services.Identity.UserServices;

public class GeneralUserService : IGeneralUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMailService _mailService;

    public GeneralUserService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IMailService mailService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _mailService = mailService;
    }
    
    public async Task<IResult> RegisterAsync(RegisterRequest request, string origin)
    {
        throw new NotImplementedException();
    }

    public async Task<string> SendVerificationEmail(AppUser user, string origin)
    {
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        const string route = "api/identity/user/confirm-email/";
        var endpointUri = new Uri(string.Concat($"{origin}/", route));
        var verificationUri = QueryHelpers.AddQueryString(endpointUri.ToString(), "userId", user.Id);
        verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
        return verificationUri;
    }

    public async Task<IResult> ToggleUserStatusAsync(ToggleUserStatusRequest request)
    {
        try
        {
            var user = await _userManager.Users.Where(u => u.Id == request.UserId).FirstOrDefaultAsync();
            if (user is null)
                throw new Exception("User not found!");
            user.IsActive = request.ActivateUser;
            var identityResult = await _userManager.UpdateAsync(user);
            return identityResult.Succeeded ? await Result.SuccessAsync() : throw new Exception();
        }
        catch (Exception e)
        {
            return await Result.FailAsync(e.Message);
        }
    }

    public async Task<IResult<UserRolesResponse>> GetRolesAsync(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<IResult> UpdateRolesAsync(UpdateUserRolesRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<IResult<string>> ConfirmEmailAsync(string userId, string code)
    {
        var user = await _userManager.FindByIdAsync(userId);
        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        var result = await _userManager.ConfirmEmailAsync(user, code);
        return result.Succeeded
            ? await Result<string>.SuccessAsync(user.Id,
                $"Account Confirmed for {user.Email}. You can now use the /api/identity/token endpoint to generate JWT.")
            : throw new Exception($"An error occurred while confirming {user.Email}");
    }

    public async Task<IResult> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
        {
            // Don't reveal that the user does not exist or is not confirmed
            return await Result.FailAsync("An Error has occurred!");
        }
        // For more information on how to enable account confirmation and password reset please
        // visit https://go.microsoft.com/fwlink/?LinkID=532713
        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var route = "account/reset-password";
        var endpointUri = new Uri(string.Concat($"{origin}/", route));
        var passwordResetUrl = QueryHelpers.AddQueryString(endpointUri.ToString(), "Token", code);
        var mailRequest = new MailRequest
        {
            Body =
                $"Please reset your password by <a href=\"{HtmlEncoder.Default.Encode(passwordResetUrl)}\">clicking here</a>.",
            Subject = "Reset Password",
            To = request.Email
        };
        await _mailService.SendMailAsync(mailRequest, origin);
        return await Result.SuccessAsync("Password Reset Mail has been sent to your authorized Email.");
    }

    public async Task<IResult> ResetPasswordAsync(ResetPasswordRequest request)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                throw new Exception("An Error has occured!");
            }
            if (user.IsDeleted) throw new Exception("An Error has occured!");
            if (!user.IsActive) throw new Exception("User not active. Please contact support");
            if (await _userManager.IsInRoleAsync(user, RoleConstants.Internal))
                throw new Exception("User not Found!");
            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
            return result.Succeeded ? await Result.SuccessAsync("Password Reset Successful!") : throw new Exception("An Error has occured!");

        }
        catch (Exception e)
        {
            return await Result.FailAsync(e.Message);
        }
    }
}