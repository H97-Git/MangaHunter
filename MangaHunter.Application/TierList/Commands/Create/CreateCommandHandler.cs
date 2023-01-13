using ErrorOr;

using MangaHunter.Application.Common.Errors;
using MangaHunter.Application.Common.Interfaces.Persistence;

using MediatR;

namespace MangaHunter.Application.TierList.Commands.Create;

public class CreateCommandHandler : IRequestHandler<CreateCommand, ErrorOr<Domain.Entities.TierList>>
{
    private readonly ITierListRepository _repository;

    public CreateCommandHandler(ITierListRepository repository)
    {
        _repository = repository;
    }

    public async Task<ErrorOr<Domain.Entities.TierList>> Handle(CreateCommand command,
        CancellationToken cancellationToken)
    {
        var tierlist = command.TierList;
        if (tierlist.UserName != command.Username)
        {
            return Error.Conflict();
        }

        if (tierlist.Id is not 0)
        {
            return Errors.TierList.Conflict;
        }

        tierlist.ShareCode = GetNewShareCode();
        return await _repository.Add(tierlist);
    }

    private string GetNewShareCode()
    {
        string sC;
        do
        {
            sC = GenerateShareCode();
        } while (!_repository.IsShareCodeNew(sC));

        return sC;
    }

    private static string GenerateShareCode()
    {
        var abcXyz = "ABCDEFGHJKMNPQRSTUVWXYZ".ToCharArray(); // -ILO
        var nums = "123456789".ToCharArray(); 
        const int maxABC = 22;
        const int maxNum = 8;
        var r = new Random();
        var char1 = r.Next(maxABC);
        var char2 = r.Next(maxNum);
        var char3 = r.Next(maxABC);
        var char4 = r.Next(maxNum);
        var char5 = r.Next(maxABC);
        var char6 = r.Next(maxNum);
        return $"{abcXyz[char1]}{nums[char2]}{abcXyz[char3]}{nums[char4]}{abcXyz[char5]}{nums[char6]}";
    }
}