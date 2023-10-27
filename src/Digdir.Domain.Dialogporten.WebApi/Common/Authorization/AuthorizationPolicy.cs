﻿namespace Digdir.Domain.Dialogporten.WebApi.Common.Authorization;

internal static class AuthorizationPolicy
{
    public const string Serviceprovider = "serviceprovider";
    public const string ServiceproviderSearch = "serviceproviderSearch";
    public const string Testing = "testing";
}

internal static class AuthorizationScope
{
    public const string Serviceprovider = "digdir:dialogporten.serviceprovider";
    public const string ServiceproviderSearch = "digdir:dialogporten.serviceprovider.search";
    public const string Testing = "digdir:dialogporten.developer.test";

    internal static readonly Lazy<IReadOnlyCollection<string>> AllScopes = new(GetAll);
    private static IReadOnlyCollection<string> GetAll() => 
        typeof(AuthorizationScope)
            .GetFields()
            .Where(x => x.IsLiteral && !x.IsInitOnly && x.DeclaringType == typeof(string))
            .Select(x => (string)x.GetRawConstantValue()!)
            .ToList();
}