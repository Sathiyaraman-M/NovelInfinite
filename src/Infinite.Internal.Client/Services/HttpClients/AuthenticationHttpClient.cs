using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using Blazored.LocalStorage;
using Infinite.Internal.Client.Authentication;
using Infinite.Internal.Client.Extensions;
using Infinite.Shared.Constants;
using Infinite.Shared.Requests.Identity;
using Infinite.Shared.Responses.Identity;
using Microsoft.AspNetCore.Components.Authorization;

namespace Infinite.Internal.Client.Services.HttpClients;

public class AuthenticationHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly ILocalStorageService _localStorageService;

    public AuthenticationHttpClient(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider, ILocalStorageService localStorageService)
    {
        _httpClient = httpClient;
        _authenticationStateProvider = authenticationStateProvider;
        _localStorageService = localStorageService;
    }

    public ClaimsPrincipal CurrentUser()
    {
        return ((InternalAppAuthenticationStateProvider)_authenticationStateProvider).AuthenticationStateUser;
    }

    public async Task<IResult> Login(TokenRequest tokenRequest)
    {
        var response = await _httpClient.PostAsJsonAsync("api/token/get", tokenRequest);
        var result = await response.ToResult<TokenResponse>();
        if (result.Succeeded)
        {
            var token = result.Data.Token;
            var refreshToken = result.Data.RefreshToken;
            var userImageUrl = result.Data.UserImageUrl;
            await _localStorageService.SetItemAsync(StorageConstants.InternalAuthToken, token);
            await _localStorageService.SetItemAsync(StorageConstants.RefreshToken, refreshToken);
            if (!string.IsNullOrEmpty(userImageUrl))
            {
                await _localStorageService.SetItemAsync(StorageConstants.UserImageUrl, userImageUrl);
            }
            ((InternalAppAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(tokenRequest.UserName);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await Result.SuccessAsync();
        }
        else
        {
            return await Result.FailAsync(result.Messages);
        }
    }

    public async Task<IResult> Logout()
    {
        await _localStorageService.RemoveItemAsync(StorageConstants.InternalAuthToken);
        await _localStorageService.RemoveItemAsync(StorageConstants.RefreshToken);
        await _localStorageService.RemoveItemAsync(StorageConstants.UserImageUrl);
        ((InternalAppAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
        _httpClient.DefaultRequestHeaders.Authorization = null;
        return await Result.SuccessAsync();
    }

    public async Task<string> RefreshToken()
    {
        var token = await _localStorageService.GetItemAsync<string>(StorageConstants.InternalAuthToken);
        var refreshToken = await _localStorageService.GetItemAsync<string>(StorageConstants.RefreshToken);

        var response = await _httpClient.PostAsJsonAsync("api/token/refresh", new RefreshTokenRequest { Token = token, RefreshToken = refreshToken });

        var result = await response.ToResult<TokenResponse>();

        if (!result.Succeeded)
        {
            throw new ApplicationException("Something went wrong during the refresh token action");
        }

        token = result.Data.Token;
        refreshToken = result.Data.RefreshToken;
        await _localStorageService.SetItemAsync(StorageConstants.InternalAuthToken, token);
        await _localStorageService.SetItemAsync(StorageConstants.RefreshToken, refreshToken);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return token;
    }

    public async Task<string> TryForceRefreshToken()
    {
        var availableToken = await _localStorageService.GetItemAsync<string>(StorageConstants.RefreshToken);
        if (string.IsNullOrEmpty(availableToken)) return string.Empty;
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        var exp = user.FindFirst(c => c.Type.Equals("exp"))?.Value;
        var expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp));
        var diff = expTime - DateTime.Now;
        if (diff.TotalMinutes <= 1)
            return await RefreshToken();
        return string.Empty;
    }

    public async Task<string> TryRefreshToken()
    {
        return await RefreshToken();
    }
}