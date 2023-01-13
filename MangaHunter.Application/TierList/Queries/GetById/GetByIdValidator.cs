using FluentValidation;

namespace MangaHunter.Application.TierList.Queries.GetById;

public class GetByIdValidator : AbstractValidator<GetByIdQuery>
{
    public GetByIdValidator()
    {
        // RuleFor(x => x.Username).NotEmpty();
        // RuleFor(x => x.MangadexId).NotEmpty();
    }
}