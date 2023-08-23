using Digdir.Domain.Dialogporten.Application.Features.V1.EndUser.DialogElements.Queries.Get;
using FastEndpoints;
using MediatR;

namespace Digdir.Domain.Dialogporten.WebApi.Endpoints.V1.EndUser.DialogElement;

public class GetDialogElementEndpoint : Endpoint<GetDialogElementQuery>
{
    private readonly ISender _sender;

    public GetDialogElementEndpoint(ISender sender)
    {
        _sender = sender ?? throw new ArgumentNullException(nameof(sender));
    }

    public override void Configure()
    {
        Get("dialogs/{dialogId}/dialogelements/{dialogElementId}");
        Group<EndUserGroup>();
    }

    public override async Task HandleAsync(GetDialogElementQuery req, CancellationToken ct)
    {
        var result = await _sender.Send(req, ct);
        await result.Match(
            dto => SendOkAsync(dto, ct),
            notFound => this.NotFoundAsync(notFound, ct),
            deleted => this.GoneAsync(deleted, ct));
    }
}