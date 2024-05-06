using Digdir.Domain.Dialogporten.Application.Common.Extensions;
using Digdir.Domain.Dialogporten.Application.Externals.AltinnAuthorization;
using Digdir.Domain.Dialogporten.Application.Externals.Presentation;
using Digdir.Domain.Dialogporten.Domain.Parties;

namespace Digdir.Domain.Dialogporten.Application.Common;

public interface IUserParties
{
    public Task<AuthorizedPartiesResult> GetUserParties(CancellationToken cancellationToken = default);
}

public class UserParties : IUserParties
{
    private readonly IUser _user;
    private readonly IAltinnAuthorization _altinnAuthorization;

    public UserParties(IUser user, IAltinnAuthorization altinnAuthorization)
    {
        _user = user ?? throw new ArgumentNullException(nameof(user));
        _altinnAuthorization = altinnAuthorization ?? throw new ArgumentNullException(nameof(altinnAuthorization));
    }

    public Task<AuthorizedPartiesResult> GetUserParties(CancellationToken cancellationToken = default) =>
        _user.TryGetPid(out var pid) &&
        NorwegianPersonIdentifier.TryParse(NorwegianPersonIdentifier.PrefixWithSeparator + pid,
            out var partyIdentifier)
            ? _altinnAuthorization.GetAuthorizedParties(partyIdentifier, cancellationToken)
            : Task.FromResult(new AuthorizedPartiesResult());
}
