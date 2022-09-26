using MudBlazor;

namespace Infinite.Client.Services;

public class AppearanceService
{
    public MudTheme AppTheme { get; set; } = new();
    
    public bool IsDarkMode { get; set; } = false;
}