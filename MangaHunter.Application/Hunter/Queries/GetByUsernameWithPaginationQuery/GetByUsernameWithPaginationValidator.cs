using FluentValidation;

namespace MangaHunter.Application.Hunter.Queries.GetByUsernameWithPaginationQuery;

public class GetByUsernameWithPaginationValidator : AbstractValidator<GetByUsernameWithPaginationQuery>
{
    public GetByUsernameWithPaginationValidator()
    {
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.PaginationParameters.Size).LessThanOrEqualTo(100);
        RuleFor(x => x.PaginationParameters.ExcludedStatus)
            .Equal(false)
            .When(x => string.IsNullOrEmpty(x.PaginationParameters.Status))
            .WithMessage("Can't exclude all status");

    }
}