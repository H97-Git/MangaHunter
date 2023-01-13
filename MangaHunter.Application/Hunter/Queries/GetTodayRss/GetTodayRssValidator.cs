using FluentValidation;

namespace MangaHunter.Application.Hunter.Queries.GetTodayRss;

public class GetTodayRssValidator : AbstractValidator<GetTodayRssQuery>
{
    public GetTodayRssValidator()
    {
        // RuleFor(x => x.Username).NotEmpty();
    }
}