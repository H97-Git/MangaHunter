using ErrorOr;

using MediatR;

namespace MangaHunter.Application.TierList.Commands.Update;

public record UpdateCommand(Domain.Entities.TierList TierList, int Id, string Username) : IRequest<ErrorOr<Updated>>;