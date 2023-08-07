﻿using Digdir.Domain.Dialogporten.Application.Features.V1.Dialogs.Queries.ServiceOwner.Get;
using FastEndpoints;
using MediatR;

namespace Digdir.Domain.Dialogporten.WebApi.Endpoints.V1.ServiceOwner.Dialog;

public class GetDialogEndpoint : Endpoint<GetDialogQuery>
{
    private readonly ISender _sender;

    public GetDialogEndpoint(ISender sender)
    {
        _sender = sender ?? throw new ArgumentNullException(nameof(sender));
    }

    public override void Configure()
    {
        Get("dialogs/{id}");
        Group<ServiceOwnerGroup>();
    }

    public override async Task HandleAsync(GetDialogQuery req, CancellationToken ct)
    {
        var result = await _sender.Send(req, ct);
        await result.Match(
            dto =>
            {
                HttpContext.Response.Headers.ETag = dto.ETag.ToString();
                return SendOkAsync(dto, ct);
            },
            notFound => this.NotFoundAsync(notFound, ct));
    }
}