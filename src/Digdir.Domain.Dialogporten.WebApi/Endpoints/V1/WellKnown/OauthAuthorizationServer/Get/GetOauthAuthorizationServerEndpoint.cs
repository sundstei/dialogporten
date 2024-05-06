using Digdir.Domain.Dialogporten.Application.Features.V1.WellKnown.OauthAuthorizationServer.Queries.Get;
using FastEndpoints;
using MediatR;
using Microsoft.Net.Http.Headers;

namespace Digdir.Domain.Dialogporten.WebApi.Endpoints.V1.WellKnown.OauthAuthorizationServer.Get;

public sealed class GetOauthAuthorizationServerEndpoint : EndpointWithoutRequest<GetOauthAuthorizationServerDto>
{
    private readonly ISender _sender;

    public GetOauthAuthorizationServerEndpoint(ISender sender)
    {
        _sender = sender ?? throw new ArgumentNullException(nameof(sender));
    }

    public override void Configure()
    {
        Get(".well-known/oauth-authorization-server");
        Group<MetadataGroup>();

        Description(b => GetOauthAuthorizationServerSwaggerConfig.SetDescription(b));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await _sender.Send(new GetOauthAuthorizationServerQuery(), ct);

        HttpContext.Response.GetTypedHeaders().CacheControl = new CacheControlHeaderValue
        {
            Public = true,
            MaxAge = TimeSpan.FromHours(24)
        };
        HttpContext.Response.Headers[HeaderNames.Vary] = new[] { "Accept-Encoding" };

        await SendOkAsync(result, ct);
    }
}
