using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Digdir.Domain.Dialogporten.Infrastructure.Common.Exceptions;

namespace Digdir.Domain.Dialogporten.Infrastructure;

public static class HttpClientExtensions
{
    public static async Task<T> GetFromJsonEnsuredAsync<T>(
        this HttpClient client,
        string requestUri,
        Action<HttpRequestHeaders>? configureHeaders = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
            configureHeaders?.Invoke(httpRequestMessage.Headers);
            var response = await client.SendAsync(httpRequestMessage, cancellationToken);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);
            return result is null
                ? throw new JsonException($"Failed to deserialize JSON to type {typeof(T).FullName} from {requestUri}")
                : result;
        }
        catch (Exception e)
        {
            throw new UpstreamServiceException(e);
        }
    }

    public static async Task<HttpResponseMessage> PostAsJsonEnsuredAsync(
        this HttpClient client,
        string requestUri,
        object content,
        Action<HttpRequestHeaders>? configureHeaders = null,
        Action<HttpContentHeaders>? configureContentHeaders = null,
        JsonSerializerOptions? serializerOptions = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Content = JsonContent.Create(content, options: serializerOptions)
            };
            configureHeaders?.Invoke(httpRequestMessage.Headers);
            configureContentHeaders?.Invoke(httpRequestMessage.Content.Headers);
            var response = await client.SendAsync(httpRequestMessage, cancellationToken);
            response.EnsureSuccessStatusCode();
            return response;
        }
        catch (Exception e)
        {
            throw new UpstreamServiceException(e);
        }
    }

    public static async Task<T> PostAsJsonEnsuredAsync<T>(
        this HttpClient client,
        string requestUri,
        object content,
        Action<HttpRequestHeaders>? configureHeaders = null,
        Action<HttpContentHeaders>? configureContentHeaders = null,
        JsonSerializerOptions? serializerOptions = null,
        CancellationToken cancellationToken = default)
    {
        var response = await client.PostAsJsonEnsuredAsync(requestUri, content, configureHeaders,
            configureContentHeaders, serializerOptions, cancellationToken);
        try
        {
            var result = await response.Content.ReadFromJsonAsync<T>(cancellationToken: cancellationToken);
            return result is null
                ? throw new JsonException($"Failed to deserialize JSON to type {typeof(T).FullName} from {requestUri}")
                : result;
        }
        catch (Exception e)
        {
            throw new UpstreamServiceException(e);
        }
    }
}
