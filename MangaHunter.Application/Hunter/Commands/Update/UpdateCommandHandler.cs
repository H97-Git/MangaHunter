using ErrorOr;

using MangaHunter.Application.Common.Errors;
using MangaHunter.Application.Common.Interfaces.Persistence;

using MapsterMapper;

using MediatR;

namespace MangaHunter.Application.Hunter.Commands.Update;

public class UpdateCommandHandler : IRequestHandler<UpdateCommand, ErrorOr<Domain.Entities.Hunter>>
{
    private readonly IHunterRepository _repository;
    private readonly IMapper _mapper;

    public UpdateCommandHandler(IHunterRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ErrorOr<Domain.Entities.Hunter>> Handle(UpdateCommand command,
        CancellationToken cancellationToken)
    {
        var hunterInRepo = await _repository.GetByMangaIdNoCaching(new Guid(command.MangadexId), command.Username);
        if (hunterInRepo is null)
        {
            return Errors.Hunter.NotFound;
        }

        if (hunterInRepo.Id != command.Hunter.Id)
        {
            return Errors.Hunter.Conflict;
        }

        // Hiding property on dto mean it can't be mapped.
        var createdOnUtc = hunterInRepo.CreatedOnUtc;
        hunterInRepo = _mapper.Map<Domain.Entities.Hunter>(command.Hunter);
        hunterInRepo.CreatedOnUtc = createdOnUtc;
        return await _repository.Update(hunterInRepo);
    }
}