using FluentValidation;

namespace MangaHunter.Application.TierList.Queries.GetByUsername;

public class GetByUsernameValidator : AbstractValidator<GetByUsernameQuery>
{
    public GetByUsernameValidator()
    {
        // RuleFor(x => x.Username).NotEmpty();
        // RuleFor(x => x.MangadexId).NotEmpty();
    }
}