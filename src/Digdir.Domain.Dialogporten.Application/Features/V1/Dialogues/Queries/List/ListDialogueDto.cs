﻿using Digdir.Domain.Dialogporten.Application.Features.V1.Common.Localizations;
using Digdir.Domain.Dialogporten.Domain.Dialogues.Entities;

namespace Digdir.Domain.Dialogporten.Application.Features.V1.Dialogues.Queries.List;

public sealed class ListDialogueDto
{
    public Guid Id { get; set; }
    public string Org { get; set; } = null!;
    public string ServiceResourceIdentifier { get; set; } = null!;
    public string Party { get; set; } = null!;
    public string? ExtendedStatus { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset UpdatedAtUtc { get; set; }
    public DateTimeOffset? DueAtUtc { get; set; }

    // TODO: Denne må vi finne ut hvordan vi mapper
    public DialogueStatus.Enum StatusId { get; set; }
    public List<LocalizationDto> Title { get; set; } = new();
    public List<LocalizationDto> SenderName { get; set; } = new();
}