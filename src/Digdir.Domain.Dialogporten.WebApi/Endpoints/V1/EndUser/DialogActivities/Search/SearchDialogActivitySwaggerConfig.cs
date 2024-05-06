using Digdir.Domain.Dialogporten.Application.Features.V1.EndUser.DialogActivities.Queries.Search;
using Digdir.Domain.Dialogporten.WebApi.Common;
using Digdir.Domain.Dialogporten.WebApi.Common.Extensions;
using Digdir.Domain.Dialogporten.WebApi.Common.Swagger;
using Digdir.Domain.Dialogporten.WebApi.Endpoints.V1.Common.Extensions;
using FastEndpoints;

namespace Digdir.Domain.Dialogporten.WebApi.Endpoints.V1.EndUser.DialogActivities.Search;

public class SearchDialogActivitySwaggerConfig : ISwaggerConfig
{
    public static string OperationId => "GetDialogActivityList";

    public static RouteHandlerBuilder SetDescription(RouteHandlerBuilder builder)
        => builder.OperationId(OperationId);

    public static object GetExample() => throw new NotImplementedException();
}

public sealed class SearchDialogActivityEndpointSummary : Summary<SearchDialogActivityEndpoint, SearchDialogActivityQuery>
{
    public SearchDialogActivityEndpointSummary()
    {
        Summary = "Gets a list of dialog activities";
        Description = """
                      Gets the list of activities belonging to a dialog
                      """;
        Responses[StatusCodes.Status200OK] = Constants.SwaggerSummary.ReturnedResult.FormatInvariant("activity list");
        Responses[StatusCodes.Status401Unauthorized] = Constants.SwaggerSummary.EndUserAuthenticationFailure;
        Responses[StatusCodes.Status403Forbidden] = Constants.SwaggerSummary.AccessDeniedToDialog.FormatInvariant("get");
        Responses[StatusCodes.Status404NotFound] = Constants.SwaggerSummary.DialogNotFound;
    }
}
