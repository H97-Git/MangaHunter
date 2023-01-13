using ErrorOr;

using MangaHunter.Application.Common.Errors;
using MangaHunter.Application.Common.Interfaces.Persistence;

using MapsterMapper;

using MediatR;

using Serilog;

namespace MangaHunter.Application.TierList.Commands.Update;

public class UpdateCommandHandler : IRequestHandler<UpdateCommand, ErrorOr<Updated>>
{
    private readonly ITierListRepository _repository;
    private readonly IMapper _mapper;

    public UpdateCommandHandler(ITierListRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ErrorOr<Updated>> Handle(UpdateCommand command,
        CancellationToken cancellationToken)
    {
        var tierlistInRepo = await _repository.GetByIdNoCaching(command.Id);
        if (tierlistInRepo is null)
        {
            return Errors.TierList.NotFound;
        }

        var shareCode = tierlistInRepo.ShareCode; //Save share code in case of modification.

        foreach (var row in tierlistInRepo.Tiers)
        {
            // If there is no match with command.tiers and tierlistInRepo.tiers this tier should be deleted
            if (!command.TierList.Tiers.Any(x => x.Identifier.Equals(row.Identifier)))
            {
                _repository.DeleteRow(row);
            }
        }

        tierlistInRepo = _mapper.Map<Domain.Entities.TierList>(command.TierList);
        tierlistInRepo.ShareCode = shareCode;
        var ids = tierlistInRepo.AllIdsInTierList;
        var allDuplicates = ids.GroupBy(x => x).Where(g => g.Count() > 1);
        if (allDuplicates.Any())
        {
            return Errors.TierList.Duplicates;
        }

        try
        {
            return await _repository.Update(tierlistInRepo);
        }
        catch (Exception e)
        {
            Log.Error("Failed to update the tierlist", e);
            return Errors.TierList.Unexpected;
        }
    }
}