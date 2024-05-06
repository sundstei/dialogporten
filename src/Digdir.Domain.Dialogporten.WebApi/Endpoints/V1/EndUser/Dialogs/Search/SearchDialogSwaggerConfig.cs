using Digdir.Domain.Dialogporten.Application.Common.Pagination;
using Digdir.Domain.Dialogporten.Application.Features.V1.EndUser.Dialogs.Queries.Search;
using Digdir.Domain.Dialogporten.WebApi.Common.Extensions;
using Digdir.Domain.Dialogporten.WebApi.Common.Swagger;
using Digdir.Domain.Dialogporten.WebApi.Common;
using Digdir.Domain.Dialogporten.WebApi.Endpoints.V1.Common.Extensions;
using FastEndpoints;

namespace Digdir.Domain.Dialogporten.WebApi.Endpoints.V1.EndUser.Dialogs.Search;

public abstract class SearchDialogSwaggerConfig : ISwaggerConfig
{
    public static string OperationId => "GetDialogList";

    public static RouteHandlerBuilder SetDescription(RouteHandlerBuilder builder) =>
        builder.OperationId(OperationId)
            .ClearDefaultProduces(StatusCodes.Status403Forbidden);

    public static object GetExample() => throw new NotImplementedException();
}

public sealed class SearchDialogEndpointSummary : Summary<SearchDialogEndpoint, SearchDialogQuery>
{
    public SearchDialogEndpointSummary()
    {
        Summary = "Gets a list of dialogs";
        Description = """
                      Performs a search for dialogs, returning a paginated list of dialogs. For more information see the documentation (link TBD).

                      * All date parameters must contain explicit time zone. Example: 2023-10-27T10:00:00Z or 2023-10-27T10:00:00+01:00
                      * See "continuationToken" in the response for how to get the next page of results.
                      * hasNextPage will be set to true if there are more items to get.
                      """;

        Responses[StatusCodes.Status200OK] = Constants.SwaggerSummary.ReturnedResult.FormatInvariant("list");
        Responses[StatusCodes.Status401Unauthorized] = Constants.SwaggerSummary.EndUserAuthenticationFailure;

        RequestParam(p => p.ContinuationToken, "Supply \"continuationToken\" for the response to get the next page of results, if hasNextPage is true");
        RequestParam(p => p.Limit, $"Limit the number of results per page ({PaginationConstants.MinLimit}-{PaginationConstants.MaxLimit}, default: {PaginationConstants.DefaultLimit})");
    }
}
