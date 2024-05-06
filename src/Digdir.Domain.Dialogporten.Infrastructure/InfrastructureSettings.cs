﻿using Altinn.ApiClients.Maskinporten.Config;
using Digdir.Domain.Dialogporten.Application.Common.Extensions.FluentValidation;
using FluentValidation;

namespace Digdir.Domain.Dialogporten.Infrastructure;

public sealed class InfrastructureSettings
{
    public const string ConfigurationSectionName = "Infrastructure";

    public required string DialogDbConnectionString { get; init; }
    public required RedisSettings Redis { get; init; }
    public required AltinnPlatformSettings Altinn { get; init; }
    public required AltinnCdnPlatformSettings AltinnCdn { get; init; }
    public required MaskinportenSettings Maskinporten { get; init; }
}

public sealed class AltinnPlatformSettings
{
    public required Uri BaseUri { get; init; }
    public required Uri EventsBaseUri { get; init; }
    public required string SubscriptionKey { get; init; }
}

public sealed class AltinnCdnPlatformSettings
{
    public required Uri BaseUri { get; init; }
}

public sealed class RedisSettings
{
    public required bool? Enabled { get; init; }
    public required string ConnectionString { get; init; }
}

internal sealed class InfrastructureSettingsValidator : AbstractValidator<InfrastructureSettings>
{
    public InfrastructureSettingsValidator(
        IValidator<AltinnPlatformSettings> altinnPlatformSettingsValidator,
        IValidator<AltinnCdnPlatformSettings> altinnCdnPlatformSettingsValidator,
        IValidator<MaskinportenSettings> maskinportenSettingsValidator,
        IValidator<RedisSettings> redisSettingsValidator)
    {
        RuleFor(x => x.DialogDbConnectionString)
            .NotEmpty();

        RuleFor(x => x.Altinn)
            .NotEmpty()
            .SetValidator(altinnPlatformSettingsValidator);

        RuleFor(x => x.AltinnCdn)
            .NotEmpty()
            .SetValidator(altinnCdnPlatformSettingsValidator);

        RuleFor(x => x.Maskinporten)
            .NotEmpty()
            .SetValidator(maskinportenSettingsValidator);

        RuleFor(x => x.Redis)
            .NotEmpty()
            .SetValidator(redisSettingsValidator);
    }
}

internal sealed class AltinnPlatformSettingsValidator : AbstractValidator<AltinnPlatformSettings>
{
    public AltinnPlatformSettingsValidator()
    {
        RuleFor(x => x.BaseUri).NotEmpty().IsValidUri();
    }
}

internal sealed class AltinnCdnPlatformSettingsValidator : AbstractValidator<AltinnCdnPlatformSettings>
{
    public AltinnCdnPlatformSettingsValidator()
    {
        RuleFor(x => x.BaseUri).NotEmpty().IsValidUri();
    }
}

internal sealed class MaskinportenSettingsValidator : AbstractValidator<MaskinportenSettings>
{
    public MaskinportenSettingsValidator()
    {
        RuleFor(x => x.ClientId).NotEmpty();
        RuleFor(x => x.Scope).NotEmpty();
        RuleFor(x => x.Environment).NotEmpty();
        RuleFor(x => x.EncodedJwk).NotEmpty();
    }
}

internal sealed class RedisSettingsValidator : AbstractValidator<RedisSettings>
{
    public RedisSettingsValidator()
    {
        RuleFor(x => x.Enabled).Must(x => x is false or true);

        When(x => x.Enabled == true, () =>
        {
            RuleFor(x => x.ConnectionString).NotEmpty();
        });
    }
}