using ErrorOr;

using MediatR;

namespace MangaHunter.Application.Hunter.Commands.Create;

public record CreateCommand(Domain.Entities.Hunter Hunter,string Username) : IRequest<ErrorOr<Domain.Entities.Hunter>>;