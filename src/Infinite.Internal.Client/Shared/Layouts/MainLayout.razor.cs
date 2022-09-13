namespace Infinite.Internal.Client.Shared.Layouts;

public partial class MainLayout
{
    private async Task Logout()
    {
        if(await DialogService.ShowMessageBox("Log out?", "Are you sure want to logout?", "Yes", "No") == true)
        {
            await AuthenticationHttpClient.Logout();
            NavigationManager.NavigateTo("/");
        }
    }
}