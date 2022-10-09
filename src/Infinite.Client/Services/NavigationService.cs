using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Infinite.Client.Services;

public class NavigationService : IDisposable
{
    private const int MinHistorySize = 256;
    private const int AdditionalHistorySize = 64;
    private readonly NavigationManager _navigationManager;
    private readonly List<string> _history;

    public NavigationService(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
        _history = new List<string>(MinHistorySize + AdditionalHistorySize)
        {
            _navigationManager.ToBaseRelativePath(_navigationManager.Uri)
        };
        _navigationManager.LocationChanged += NavigationManagerOnLocationChanged;
    }

    public bool CanNavigateBack => _history.Count >= 2;


    public void NavigateTo(string url)
    {
        _navigationManager.NavigateTo(url);
    }

    public void NavigateBack()
    {
        if (!CanNavigateBack)
            return;
        var backPageUrl = _history[^2];
        _history.RemoveRange(_history.Count - 2, 2);
        _navigationManager.NavigateTo(backPageUrl);
    }

    private void NavigationManagerOnLocationChanged(object sender, LocationChangedEventArgs e)
    {
        EnsureSize();
        if (_navigationManager.ToBaseRelativePath(e.Location) == "administration/dashboard" || _navigationManager.ToBaseRelativePath(e.Location) == "")
        {
            _history.Clear();
        }
        _history.Add(_navigationManager.ToBaseRelativePath(e.Location));
    }

    private void EnsureSize()
    {
        if (_history.Count < MinHistorySize + AdditionalHistorySize)
            return;
        _history.RemoveRange(0, _history.Count - MinHistorySize);
    }

    public void Dispose()
    {
        _navigationManager.LocationChanged -= NavigationManagerOnLocationChanged;
    }
}