using ErrorOr;

using MediatR;

namespace MangaHunter.Application.TierList.Queries.GetByUsername;

public record GetByUsernameQuery(string Username) : IRequest<ErrorOr<List<Domain.Entities.TierList>>>;