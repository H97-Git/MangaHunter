using FluentValidation;

namespace MangaHunter.Application.Hunter.Commands.Create;

public class CreateCommandValidator : AbstractValidator<CreateCommand>
{
    public CreateCommandValidator()
    {
        RuleFor(x => x.Username)
            .Equal(x => x.Hunter.Username)
            .WithMessage("Conflict with username.");
    }
}