using FluentValidation;

using MangaHunter.Application.Hunter.Queries.GetById;

namespace MangaHunter.Application.Hunter.Queries.GetByUsername;

public class GetByUsernameValidator : AbstractValidator<GetByIdQuery>
{
    public GetByUsernameValidator()
    {
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.MangadexId).NotEmpty();
    }
}