using FluentValidation;

namespace MangaHunter.Application.Hunter.Queries.GetMangaUpdatesId;

public class GetNewMangaUpdatesIdValidator : AbstractValidator<GetNewMangaUpdatesIdQuery>
{
    public GetNewMangaUpdatesIdValidator()
    {
        RuleFor(x => x.MangadexId).NotEmpty();
    }
}