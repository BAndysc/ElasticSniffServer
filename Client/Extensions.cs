using System.Net.Http.Json;
using System.Text.Json;

namespace SearchSniffServer.Client;

internal static class Extensions
{
    public static HttpResponseMessage PostAsJson<TValue>(this HttpClient client, Uri requestUri, TValue value, JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
    {
        if (client == null)
        {
            throw new ArgumentNullException(nameof(client));
        }

        JsonContent content = JsonContent.Create(value, mediaType: null, options);
        return client.Send(new HttpRequestMessage(HttpMethod.Post, requestUri)
        {
            Content = content
        }, cancellationToken);
    }
}