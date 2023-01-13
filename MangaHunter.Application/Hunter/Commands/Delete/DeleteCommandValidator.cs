using FluentValidation;

namespace MangaHunter.Application.Hunter.Commands.Delete;

public class DeleteCommandValidator : AbstractValidator<DeleteCommand>
{
    public DeleteCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty();
        RuleFor(x => x.MangadexId)
            .NotEmpty();
    }
}