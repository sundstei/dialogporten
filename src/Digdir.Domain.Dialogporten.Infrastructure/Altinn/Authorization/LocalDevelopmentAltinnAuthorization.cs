﻿using System.Diagnostics.CodeAnalysis;
using Digdir.Domain.Dialogporten.Application.Common.Extensions;
using Digdir.Domain.Dialogporten.Application.Externals;
using Digdir.Domain.Dialogporten.Application.Externals.AltinnAuthorization;
using Digdir.Domain.Dialogporten.Domain.Dialogs.Entities;
using Digdir.Domain.Dialogporten.Domain.Parties.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Digdir.Domain.Dialogporten.Infrastructure.Altinn.Authorization;

internal sealed class LocalDevelopmentAltinnAuthorization : IAltinnAuthorization
{
    private readonly IDialogDbContext _db;

    public LocalDevelopmentAltinnAuthorization(IDialogDbContext db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    [SuppressMessage("Performance", "CA1822:Mark members as static")]
    public Task<DialogDetailsAuthorizationResult> GetDialogDetailsAuthorization(DialogEntity dialogEntity,
        CancellationToken cancellationToken = default) =>
        // Just allow everything
        Task.FromResult(new DialogDetailsAuthorizationResult { AuthorizedAltinnActions = dialogEntity.GetAltinnActions() });

    public async Task<DialogSearchAuthorizationResult> GetAuthorizedResourcesForSearch(List<string> constraintParties, List<string> serviceResources, string? endUserId,
        CancellationToken cancellationToken = default)
    {
        // constraintParties and serviceResources are passed from the client as query parameters
        // If one and/or the other is supplied, this will limit the resources and parties to the ones supplied
        var dialogData = await _db.Dialogs
            .Select(dialog => new { dialog.Party, dialog.ServiceResource })
            .WhereIf(constraintParties.Count != 0, dialog => constraintParties.Contains(dialog.Party))
            .WhereIf(serviceResources.Count != 0, dialog => serviceResources.Contains(dialog.ServiceResource))
            .Distinct()
            .ToListAsync(cancellationToken);

        // Keep the number of parties and resources reasonable
        var allParties = dialogData.Select(x => x.Party).Distinct().Take(1000).ToList();
        var allResources = dialogData.Select(x => x.ServiceResource).Distinct().Take(1000).ToList();

        var hasMorePartiesThanResources = allParties.Count > allResources.Count;
        var authorizedResources = new DialogSearchAuthorizationResult
        {
            PartiesByResources = hasMorePartiesThanResources ? allResources.ToDictionary(resource => resource, _ => allParties) : new(),
            ResourcesByParties = hasMorePartiesThanResources ? new() : allParties.ToDictionary(party => party, _ => allResources)
        };

        return authorizedResources;
    }

    public async Task<AuthorizedPartiesResult> GetAuthorizedParties(IPartyIdentifier authenticatedParty, CancellationToken cancellationToken = default)
        => await Task.FromResult(new AuthorizedPartiesResult { AuthorizedParties = [new() { Name = "Local Party", Party = authenticatedParty.FullId }] });
}
