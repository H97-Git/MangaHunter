using ErrorOr;

using MediatR;

namespace MangaHunter.Application.TierList.Commands.Create;

public record CreateCommand(Domain.Entities.TierList TierList,string Username) : IRequest<ErrorOr<Domain.Entities.TierList>>;