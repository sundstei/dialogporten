using Digdir.Domain.Dialogporten.Application.Features.V1.ServiceOwner.Dialogs.Commands.Purge;
using Digdir.Domain.Dialogporten.Application.Integration.Tests.Common;
using Digdir.Domain.Dialogporten.Domain.Dialogs.Entities;
using Digdir.Domain.Dialogporten.Domain.Dialogs.Entities.Activities;
using Digdir.Domain.Dialogporten.Domain.Dialogs.Entities.Elements;
using Digdir.Tool.Dialogporten.GenerateFakeData;
using FluentAssertions;

namespace Digdir.Domain.Dialogporten.Application.Integration.Tests.Features.V1.ServiceOwner.Dialogs.Commands;

[Collection(nameof(DialogCqrsCollectionFixture))]
public class PurgeDialogTests(DialogApplication application) : ApplicationCollectionFixture(application)
{
    [Fact]
    public async Task Purge_RemovesDialog_FromDatabase()
    {
        // Arrange
        var expectedDialogId = Guid.NewGuid();
        var createCommand = DialogGenerator.GenerateFakeDialog(id: expectedDialogId);
        var createResponse = await Application.Send(createCommand);
        createResponse.TryPickT0(out _, out _).Should().BeTrue();

        // Act
        var purgeCommand = new PurgeDialogCommand { DialogId = expectedDialogId };
        var purgeResponse = await Application.Send(purgeCommand);

        // Assert
        purgeResponse.TryPickT0(out _, out _).Should().BeTrue();

        var dialogEntities = await Application.GetDbEntities<DialogEntity>();
        dialogEntities.Should().BeEmpty();

        var dialogElements = await Application.GetDbEntities<DialogElement>();
        dialogElements.Should().BeEmpty();

        var dialogActivities = await Application.GetDbEntities<DialogActivity>();
        dialogActivities.Should().BeEmpty();
    }

    [Fact]
    public async Task Purge_ReturnsConcurrencyError_OnIfMatchDialogRevisionMismatch()
    {
        // Arrange
        var expectedDialogId = Guid.NewGuid();
        var createCommand = DialogGenerator.GenerateFakeDialog(id: expectedDialogId);
        var createResponse = await Application.Send(createCommand);
        createResponse.TryPickT0(out _, out _).Should().BeTrue();

        // Act
        var purgeCommand = new PurgeDialogCommand { DialogId = expectedDialogId, IfMatchDialogRevision = Guid.NewGuid() };
        var purgeResponse = await Application.Send(purgeCommand);

        // Assert
        purgeResponse.TryPickT2(out _, out _).Should().BeTrue();
    }

    [Fact]
    public async Task Purge_ReturnsNotFound_OnNonExistingDialog()
    {
        // Arrange
        var expectedDialogId = Guid.NewGuid();
        var createCommand = DialogGenerator.GenerateFakeDialog(id: expectedDialogId);
        await Application.Send(createCommand);
        var purgeCommand = new PurgeDialogCommand { DialogId = expectedDialogId };
        await Application.Send(purgeCommand);

        // Act
        var purgeResponse = await Application.Send(purgeCommand);

        // Assert
        purgeResponse.TryPickT1(out _, out _).Should().BeTrue();
    }
}
