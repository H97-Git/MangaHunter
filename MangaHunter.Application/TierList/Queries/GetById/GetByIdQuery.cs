using ErrorOr;

using MediatR;

namespace MangaHunter.Application.TierList.Queries.GetById;

public record GetByIdQuery(int TierListId,string Username):IRequest<ErrorOr<TierListResult>>;