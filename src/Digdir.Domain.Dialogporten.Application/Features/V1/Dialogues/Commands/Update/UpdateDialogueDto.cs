﻿using Digdir.Domain.Dialogporten.Application.Features.V1.Common.Localizations;
using Digdir.Domain.Dialogporten.Domain.Dialogues.Entities;
using Digdir.Domain.Dialogporten.Domain.Dialogues.Entities.Actions;
using Digdir.Domain.Dialogporten.Domain.Dialogues.Entities.Activities;

namespace Digdir.Domain.Dialogporten.Application.Features.V1.Dialogues.Commands.Update;

public sealed class UpdateDialogueDto
{
    public string ServiceResourceIdentifier { get; set; } = null!;
    public string Party { get; set; } = null!;
    public DialogueStatus.Enum StatusId { get; set; }
    public string? ExtendedStatus { get; set; }
    public DateTimeOffset VisibleFromUtc { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset UpdatedAtUtc { get; set; }
    public DateTimeOffset? ReadAtUtc { get; set; }
    public DateTimeOffset? DueAtUtc { get; set; }
    public List<LocalizationDto> Body { get; set; } = new();
    public List<LocalizationDto> Title { get; set; } = new();
    public List<LocalizationDto> SenderName { get; set; } = new();
    public List<LocalizationDto> SearchTitle { get; set; } = new();
    public List<UpdateDialogueDialogueAttachmentDto> Attachments { get; set; } = new();
    public List<UpdateDialogueDialogueGuiActionDto> GuiActions { get; set; } = new();
    public List<UpdateDialogueDialogueApiActionDto> ApiActions { get; set; } = new();
    public List<UpdateDialogueDialogueActivityDto> History { get; set; } = new();
    public List<UpdateDialogueDialogueTokenScopeDto> TokenScopes { get; set; } = new();
}

public sealed class UpdateDialogueDialogueTokenScopeDto
{
    public string Value { get; set; } = null!;
}

public sealed class UpdateDialogueDialogueActivityDto
{
    public Guid? Id { get; set; }
    public DateTimeOffset? CreatedAtUtc { get; set; }
    public DialogueActivityType.Enum TypeId { get; set; }
    public string? PerformedBy { get; set; }
    public string? ExtendedType { get; set; }
    public List<LocalizationDto> Description { get; set; } = new();
    public Uri? DetailsApiUrl { get; set; }
    public Uri? DetailsGuiUrl { get; set; }
}

public sealed class UpdateDialogueDialogueApiActionDto
{
    public Guid Id { get; set; }
    public string Action { get; set; } = null!;
    public string? Resource { get; set; }
    public string HttpMethod { get; set; } = null!;
    public Uri Url { get; set; } = null!;
    public Uri? DocumentationUrl { get; set; }
    public Uri? RequestSchema { get; set; }
    public Uri? ResponseSchema { get; set; }
}

public sealed class UpdateDialogueDialogueGuiActionDto
{
    public Guid Id { get; set; }
    public string Action { get; set; } = null!;
    public DialogueGuiActionType.Enum TypeId { get; set; }
    public List<LocalizationDto> Title { get; set; } = new();
    public Uri Url { get; set; } = null!;
    public string? Resource { get; set; }
    public bool IsBackChannel { get; set; }
    public bool IsDeleteAction { get; set; }
}

public sealed class UpdateDialogueDialogueAttachmentDto
{
    public Guid Id { get; set; }
    public List<LocalizationDto> DisplayName { get; set; } = new();
    public long SizeInBytes { get; set; }
    public string ContentType { get; set; } = null!;
    public Uri Url { get; set; } = null!;
    public string? Resource { get; set; }
}