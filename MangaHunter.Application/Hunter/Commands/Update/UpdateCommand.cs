using ErrorOr;

using MediatR;

namespace MangaHunter.Application.Hunter.Commands.Update;

public record UpdateCommand(Domain.Entities.Hunter Hunter,string Username,string MangadexId) : IRequest<ErrorOr<Domain.Entities.Hunter>>;