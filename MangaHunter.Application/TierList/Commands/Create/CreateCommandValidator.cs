using FluentValidation;

namespace MangaHunter.Application.TierList.Commands.Create;

public class CreateCommandValidator : AbstractValidator<CreateCommand>
{
    public CreateCommandValidator()
    {
        // RuleFor(x => x.Username)
        //     .Equal(x => x.Hunter.Username);
    }
}