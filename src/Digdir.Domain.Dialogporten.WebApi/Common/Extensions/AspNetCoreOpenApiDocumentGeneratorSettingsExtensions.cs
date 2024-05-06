﻿using Digdir.Domain.Dialogporten.Application.Common.Pagination.Continuation;
using Digdir.Domain.Dialogporten.Application.Common.Pagination.Order;
using Digdir.Domain.Dialogporten.WebApi.Common.Json;
using NJsonSchema.Generation.TypeMappers;
using NJsonSchema;
using NSwag.Generation.AspNetCore;

namespace Digdir.Domain.Dialogporten.WebApi.Common.Extensions;

internal static class AspNetCoreOpenApiDocumentGeneratorSettingsExtensions
{
    public static AspNetCoreOpenApiDocumentGeneratorSettings CleanupPaginatedLists(
        this AspNetCoreOpenApiDocumentGeneratorSettings settings)
    {
        settings.OperationProcessors.Add(new PaginatedListParametersProcessor());

        // Attempt to remove the definitions that NSwag generates for this
        foreach (var ignoreType in new[]
        {
            typeof(ContinuationTokenSet<,>),
            typeof(Order<>),
            typeof(OrderSet<,>)
        })
        {
            settings.SchemaSettings.TypeMappers.Add(new ObjectTypeMapper(ignoreType, new JsonSchema { Type = JsonObjectType.None }));
        }

        return settings;
    }

    public static AspNetCoreOpenApiDocumentGeneratorSettings AddServiceOwnerSuffixToSchemas(
        this AspNetCoreOpenApiDocumentGeneratorSettings settings)
    {
        settings.SchemaSettings.SchemaNameGenerator = new SuffixedSchemaNameGenerator();
        return settings;
    }
}
