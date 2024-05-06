using Digdir.Domain.Dialogporten.Application.Common;
using Digdir.Domain.Dialogporten.Application.Unit.Tests.Common;

namespace Digdir.Domain.Dialogporten.Application.Unit.Tests;

public class CompactJwsGeneratorTests
{
    [Fact]
    public void ValidJwsIsGenerated()
    {
        // Arrange
        var settings = new ApplicationSettings
        {
            Dialogporten = new DialogportenSettings
            {
                BaseUri = new Uri("https://unittest"),
                Ed25519KeyPairs = new Ed25519KeyPairs
                {
                    Primary = new Ed25519KeyPair
                    {
                        Kid = "unittestkeypair1",
                        PrivateComponent = "ns9Mgams90E5bCNGg9iSXONvRvASFcWF_Nb_JJ8oAEA",
                        PublicComponent = "qIn67qFQUBiwW2kv7J-5CdUCdR67CzOSnwXPBunh0d0"
                    },
                    Secondary = new Ed25519KeyPair
                    {
                        Kid = "unittestkeypair2",
                        PrivateComponent = "",
                        PublicComponent = ""
                    }
                }
            }
        };
        var options = new OptionsMock<ApplicationSettings>(settings);
        var generator = new Ed25519Generator(options);

        var payload = new Dictionary<string, object?>
        {
            { "sub", "1234567890" },
            { "name", "John Doe" },
            { "iat", 1516239022 }
        };

        // Act
        var jws = generator.GetCompactJws(payload);

        // Assert
        Assert.True(generator.VerifyCompactJws(jws));
    }
}
