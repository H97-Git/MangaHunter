using ErrorOr;

using MangaHunter.Application.Common.Errors;
using MangaHunter.Application.Common.Interfaces.Persistence;

using MediatR;

namespace MangaHunter.Application.Hunter.Commands.Delete;

public class DeleteCommandHandler : IRequestHandler<DeleteCommand, ErrorOr<Deleted>>
{
    private readonly IHunterRepository _repository;

    public DeleteCommandHandler(IHunterRepository repository)
    {
        _repository = repository;
    }

    public async Task<ErrorOr<Deleted>> Handle(DeleteCommand command,
        CancellationToken cancellationToken)
    {
        var hunter = await _repository.GetByMangaIdNoCaching(new Guid(command.MangadexId), command.Username);
        if (hunter is null)
        {
            return Errors.Hunter.NotFound;
        }
        
        return await _repository.Delete(hunter);
    }
}