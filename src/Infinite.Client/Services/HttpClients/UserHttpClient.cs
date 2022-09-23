using System.Net.Http.Json;
using Infinite.Client.Extensions;
using Infinite.Shared.Requests.Identity;

namespace Infinite.Client.Services.HttpClients;

public class UserHttpClient
{
    private readonly HttpClient _httpClient;

    public UserHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IResult> RegisterUserAsync(RegisterRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/user/register", request);
        return await response.ToResult();
    }
}