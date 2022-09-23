using Infinite.Shared.Requests.Identity;
using MudBlazor;

namespace Infinite.Client.Pages.Identity;

public partial class Register
{
    private RegisterRequest _registerUserModel = new();

    private async Task SubmitAsync()
    {
        var response = await UserHttpClient.RegisterUserAsync(_registerUserModel);
        if (response.Succeeded)
        {
            Snackbar.Add(response.Messages[0], Severity.Success);
            NavigationManager.NavigateTo("/login");
            _registerUserModel = new RegisterRequest();
        }
        else
        {
            foreach (var message in response.Messages)
            {
                Snackbar.Add(message, Severity.Error);
            }
        }
    }

    private bool _passwordVisibility;
    private InputType _passwordInput = InputType.Password;
    private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

    private void TogglePasswordVisibility()
    {
        if (_passwordVisibility)
        {
            _passwordVisibility = false;
            _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
            _passwordInput = InputType.Password;
        }
        else
        {
            _passwordVisibility = true;
            _passwordInputIcon = Icons.Material.Filled.Visibility;
            _passwordInput = InputType.Text;
        }
    }
}