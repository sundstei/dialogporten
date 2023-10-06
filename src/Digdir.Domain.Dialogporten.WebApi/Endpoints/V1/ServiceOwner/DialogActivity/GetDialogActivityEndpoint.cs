using Digdir.Domain.Dialogporten.Application.Features.V1.ServiceOwner.DialogActivities.Queries.Get;
using Digdir.Domain.Dialogporten.WebApi.Common.Authorization;
using Digdir.Domain.Dialogporten.WebApi.Common.Extensions;
using FastEndpoints;
using MediatR;

namespace Digdir.Domain.Dialogporten.WebApi.Endpoints.V1.ServiceOwner.DialogActivity;

public class GetDialogActivityEndpoint : Endpoint<GetDialogActivityQuery>
{
    private readonly ISender _sender;

    public GetDialogActivityEndpoint(ISender sender)
    {
        _sender = sender ?? throw new ArgumentNullException(nameof(sender));
    }

    public override void Configure()
    {
        Get("dialogs/{dialogId}/activities/{activityId}");
        Policies(AuthorizationPolicy.Serviceprovider);
        Group<ServiceOwnerGroup>();
    }

    public override async Task HandleAsync(GetDialogActivityQuery req, CancellationToken ct)
    {
        var result = await _sender.Send(req, ct);
        await result.Match(
            dto => SendOkAsync(dto, ct),
            notFound => this.NotFoundAsync(notFound, ct));
    }
}