using ErrorOr;

using MangaHunter.Application.Common.Errors;
using MangaHunter.Application.Common.Interfaces.Persistence;

using MediatR;

namespace MangaHunter.Application.TierList.Commands.Delete;

public class DeleteCommandHandler : IRequestHandler<DeleteCommand, ErrorOr<Deleted>>
{
    private readonly ITierListRepository _repository;

    public DeleteCommandHandler(ITierListRepository repository)
    {
        _repository = repository;
    }

    public async Task<ErrorOr<Deleted>> Handle(DeleteCommand command,
        CancellationToken cancellationToken)
    {
        var tierlist = await _repository.GetByIdNoCaching(command.Id);
        if (tierlist is null)
        {
            return Errors.TierList.NotFound;
        }

        if (command.Username != tierlist.UserName)
        {
            return Errors.TierList.Conflict;
        }

        return await _repository.Delete(tierList: tierlist);
    }
}