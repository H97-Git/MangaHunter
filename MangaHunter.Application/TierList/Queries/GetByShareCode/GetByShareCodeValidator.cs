using FluentValidation;

namespace MangaHunter.Application.TierList.Queries.GetByShareCode;

public class GetByIdValidator : AbstractValidator<GetByShareCodeQuery>
{
    public GetByIdValidator()
    {
        // RuleFor(x => x.Username).NotEmpty();
        // RuleFor(x => x.MangadexId).NotEmpty();
    }
}