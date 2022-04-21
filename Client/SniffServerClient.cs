using System.Net;
using System.Net.Http.Json;
using SearchSniffServer.Models;

namespace SearchSniffServer.Client;

public class SniffServerClient : ISniffServerClient
{
    private readonly HttpClient client;
    private readonly Uri baseUri;

    public SniffServerClient(Uri baseUri, string user, string token)
    {
        this.baseUri = baseUri;
        client = new HttpClient();
        client.DefaultRequestHeaders.Add("x-user", user);
        client.DefaultRequestHeaders.Add("x-user-token", token);
    }

    public void Upload(UploadSniffRequest request)
    {
        var result = client.PostAsJson(new Uri(baseUri, "Upload"), request);
        if (result.StatusCode != HttpStatusCode.OK)
        {
            using var reader = new StreamReader(result.Content.ReadAsStream());
            throw new UploadException(result.StatusCode, reader.ReadToEnd());
        }
    }

    public async Task UploadAsync(UploadSniffRequest request)
    {
        var result = await client.PostAsJsonAsync(new Uri(baseUri, "Upload"), request);
        if (result.StatusCode != HttpStatusCode.OK)
        {
            using var reader = new StreamReader(await result.Content.ReadAsStreamAsync());
            throw new UploadException(result.StatusCode, await reader.ReadToEndAsync());
        }
    }
    
    public async Task<SniffSearchResponse> Search(RequestSniffSearch request)
    {
        var result = await client.PostAsJsonAsync(new Uri(baseUri, "Search"), request);
        if (result.StatusCode != HttpStatusCode.OK)
        {
            throw new SearchException(result.StatusCode, await result.Content.ReadAsStringAsync());
        }

        try
        {
            var response  = await result.Content.ReadFromJsonAsync<SniffSearchResponse>();
            if (response == null)
                throw new SearchException("Invalid search response");
            return response;
        }
        catch (Exception e)
        {
            throw new SearchException(e);
        }
    }

}