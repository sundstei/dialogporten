﻿using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Digdir.Domain.Dialogporten.Application.Common.Extensions;
using Digdir.Domain.Dialogporten.Application.Externals.AltinnAuthorization;

namespace Digdir.Domain.Dialogporten.Infrastructure.Altinn.Authorization;

public sealed class DialogDetailsAuthorizationRequest
{
    public required List<Claim> Claims { get; init; }
    public required string ServiceResource { get; init; }
    public required Guid DialogId { get; init; }
    public required string Party { get; init; }

    // Each action applies to a resource. This is the main resource, or another resource indicated by a authorization attribute
    // eg. "urn:altinn:subresource:some-sub-resource" or "urn:altinn:task:task_1"
    public required HashSet<AltinnAction> AltinnActions { get; init; }
}

public static class DialogDetailsAuthorizationRequestExtensions
{
    public static string GenerateCacheKey(this DialogDetailsAuthorizationRequest request)
    {
        var claimsKey = string.Join(";", request.Claims.GetIdentifyingClaims()
            .Select(c => $"{c.Type}:{c.Value}"));

        var actionsKey = string.Join(";", request.AltinnActions.OrderBy(a => a.Name)
            .Select(a => $"{a.Name}:{a.AuthorizationAttribute}"));

        var rawKey = $"{request.DialogId}||{claimsKey}|{actionsKey}";

        var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawKey));
        var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();

        return $"auth:details:{hashString}";
    }
}
