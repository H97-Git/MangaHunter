using ErrorOr;

using MediatR;

namespace MangaHunter.Application.TierList.Commands.Delete;

public record DeleteCommand(string Username,int Id) : IRequest<ErrorOr<Deleted>>;