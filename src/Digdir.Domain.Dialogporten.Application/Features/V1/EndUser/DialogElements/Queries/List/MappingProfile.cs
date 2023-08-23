using AutoMapper;
using Digdir.Domain.Dialogporten.Domain.Dialogs.Entities.DialogElements;

namespace Digdir.Domain.Dialogporten.Application.Features.V1.EndUser.DialogElements.Queries.List;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<DialogElement, ListDialogElementDto>();
    }
}