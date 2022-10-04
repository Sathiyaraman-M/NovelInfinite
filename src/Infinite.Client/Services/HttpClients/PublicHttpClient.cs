namespace Infinite.Client.Services.HttpClients;

public class PublicHttpClient
{
    public HttpClient HttpClient { get; }

    public PublicHttpClient(HttpClient httpClient)
    {
        HttpClient = httpClient;
    }
}