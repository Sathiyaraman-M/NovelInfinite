using System.Security.Claims;
using Infinite.Internal.Client.Extensions;

namespace Infinite.Internal.Client.Shared.Layouts;

public partial class MainLayout
{
    private bool _open;
    private ClaimsPrincipal _user;
    private string _userName = "";
    private string _fullName = "";
    private string _firstLetterOfName;
    protected override async Task OnInitializedAsync()
    {
        var state = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        _user = state.User;
        _userName = _user.GetUserName();
        _fullName = _user.GetFullName();
        _firstLetterOfName = !string.IsNullOrEmpty(_userName) ? _userName[0].ToString() : "A";
        await base.OnInitializedAsync();
    }

    private async Task Logout()
    {
        if(await DialogService.ShowMessageBox("Log out?", "Are you sure want to logout?", "Yes", "No") == true)
        {
            await AuthenticationHttpClient.Logout();
            NavigationManager.NavigateTo("/", true);
        }
    }
}