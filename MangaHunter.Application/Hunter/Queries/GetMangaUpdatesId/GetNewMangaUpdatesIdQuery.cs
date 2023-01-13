using ErrorOr;

using MediatR;

namespace MangaHunter.Application.Hunter.Queries.GetMangaUpdatesId;

public record GetNewMangaUpdatesIdQuery(string MangadexId):IRequest<ErrorOr<long?>>;