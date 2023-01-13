using FluentValidation;

namespace MangaHunter.Application.Hunter.Queries.Search;

public class SearchValidator : AbstractValidator<SearchQuery>
{
    public SearchValidator()
    {
        RuleFor(x => x.Parameters.Amount).GreaterThan(0);
    }
}