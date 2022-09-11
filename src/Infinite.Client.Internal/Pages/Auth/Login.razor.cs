using Infinite.Shared.Requests.Identity;
using MudBlazor;

namespace Infinite.Client.Internal.Pages.Auth;

public partial class Login
{
    // protected override async Task OnInitializedAsync()
    // {
    //     var user = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User;
    //     if (user.Identity?.IsAuthenticated ?? false)
    //     {
    //         NavigationManager.NavigateTo("/");
    //     }
    // }

    private readonly TokenRequest _loginModel = new TokenRequest();

    private static void SubmitAsync()
    {
        // var result = await loginHttpClient.Login(_loginModel);
        // if (result.Succeeded)
        // {
        //     Snackbar.Add(result.Messages[0], Severity.Success);
        //     NavigationManager.NavigateTo("/");
        // }
        // else
        // {
        //     foreach (var message in result.Messages)
        //     {
        //         Snackbar.Add(message, Severity.Error);
        //     }
        // }
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