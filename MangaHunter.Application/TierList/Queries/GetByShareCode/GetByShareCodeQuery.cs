using ErrorOr;

using MediatR;

namespace MangaHunter.Application.TierList.Queries.GetByShareCode;

public record GetByShareCodeQuery(string ShareCode):IRequest<ErrorOr<TierListResult>>;