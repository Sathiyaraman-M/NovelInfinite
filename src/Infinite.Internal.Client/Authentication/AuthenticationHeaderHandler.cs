using System.Net.Http.Headers;
using Blazored.LocalStorage;
using Infinite.Shared.Constants;

namespace Infinite.Internal.Client.Authentication;

public class AuthenticationHeaderHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorageService;

    public AuthenticationHeaderHandler(ILocalStorageService localStorageService) => _localStorageService = localStorageService;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Headers.Authorization?.Scheme == "Bearer") return await base.SendAsync(request, cancellationToken);
        var savedToken = await _localStorageService.GetItemAsync<string>(StorageConstants.InternalAuthToken);
        if (!string.IsNullOrWhiteSpace(savedToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);
        }
        return await base.SendAsync(request, cancellationToken);

    }
}