﻿using Digdir.Domain.Dialogporten.Application.Features.V1.ServiceOwner.Dialogs.Commands.Update;
using Digdir.Domain.Dialogporten.Application.Features.V1.ServiceOwner.Dialogs.Queries.Get;
using Digdir.Domain.Dialogporten.WebApi.Common;
using Digdir.Domain.Dialogporten.WebApi.Common.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using DialogportenAuthorizationPolicy = Digdir.Domain.Dialogporten.WebApi.Common.Authorization.AuthorizationPolicy;
using IMapper = AutoMapper.IMapper;
using ProblemDetails = FastEndpoints.ProblemDetails;

namespace Digdir.Domain.Dialogporten.WebApi.Endpoints.V1.ServiceOwner.Dialogs.Patch;

/* Since minimal apis don't support json patch documents out of the box,
 * and we've not been able to find a good alternative library for it yet,
 * we've decided to use the old way of doing it using controllers and
 * NewtonsoftJson. System.Text.Json will still be used for everything
 * else. */

[ApiController]
[Route("api/v1/serviceowner/dialogs")]
[Tags(ServiceOwnerGroup.RoutePrefix)]
[Authorize(Policy = DialogportenAuthorizationPolicy.ServiceProvider)]
public sealed class PatchDialogsController : ControllerBase
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public PatchDialogsController(ISender sender, IMapper mapper)
    {
        _sender = sender ?? throw new ArgumentNullException(nameof(sender));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// Patch a single dialog
    /// </summary>
    /// <remarks>
    /// Patches a dialog aggregate with a RFC6902 JSON Patch document. The patch document must be a JSON array of RFC6902 operations.
    /// See [https://tools.ietf.org/html/rfc6902](https://tools.ietf.org/html/rfc6902) for more information.
    ///
    /// Optimistic concurrency control is implemented using the If-Match header. Supply the Revision value from the GetDialog endpoint to ensure that the dialog is not modified/deleted by another request in the meantime.
    /// </remarks>
    /// <response code="204">Patch was successfully applied.</response>
    /// <response code="400">Validation error occured. See problem details for a list of errors.</response>
    /// <response code="401">Missing or invalid authentication token. Requires a Maskinporten-token with the scope \"digdir:dialogporten.serviceprovider\"</response>
    /// <response code="403">Unauthorized to update a dialog for the given serviceResource (not owned by authenticated organization or has additional scope requirements defined in policy)</response>
    /// <response code="404">The given dialog ID was not found or is deleted</response>
    /// <response code="412">The supplied Revision does not match the current Revision of the dialog</response>
    /// <response code="422">Domain error occured. See problem details for a list of errors.</response>
    [HttpPatch("{dialogId}")]

    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status412PreconditionFailed)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Patch(
        [FromRoute] Guid dialogId,
        [FromHeader(Name = Constants.IfMatch)] Guid? etag,
        [FromBody] JsonPatchDocument<UpdateDialogDto> patchDocument,
        CancellationToken ct)
    {
        var dialogQueryResult = await _sender.Send(new GetDialogQuery { DialogId = dialogId }, ct);
        if (dialogQueryResult.TryPickT1(out var entityNotFound, out var dialog))
        {
            return NotFound(HttpContext.ResponseBuilder(StatusCodes.Status404NotFound, entityNotFound.ToValidationResults()));
        }

        var updateDialogDto = _mapper.Map<UpdateDialogDto>(dialog);
        patchDocument.ApplyTo(updateDialogDto, ModelState);
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var command = new UpdateDialogCommand { Id = dialogId, IfMatchDialogRevision = etag, Dto = updateDialogDto };
        var result = await _sender.Send(command, ct);
        return result.Match(
            success => (IActionResult)NoContent(),
            notFound => NotFound(HttpContext.ResponseBuilder(StatusCodes.Status404NotFound, notFound.ToValidationResults())),
            badRequest => BadRequest(HttpContext.ResponseBuilder(StatusCodes.Status400BadRequest, badRequest.ToValidationResults())),
            validationFailed => BadRequest(HttpContext.ResponseBuilder(StatusCodes.Status400BadRequest, validationFailed.Errors.ToList())),
            domainError => UnprocessableEntity(HttpContext.ResponseBuilder(StatusCodes.Status422UnprocessableEntity, domainError.ToValidationResults())),
            concurrencyError => new ObjectResult(HttpContext.ResponseBuilder(StatusCodes.Status412PreconditionFailed)) { StatusCode = StatusCodes.Status412PreconditionFailed }
        );
    }
}
