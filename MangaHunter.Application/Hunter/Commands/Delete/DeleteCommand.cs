using ErrorOr;

using MediatR;

namespace MangaHunter.Application.Hunter.Commands.Delete;

public record DeleteCommand(string Username,string MangadexId) : IRequest<ErrorOr<Deleted>>;