using FluentValidation;

namespace MangaHunter.Application.Hunter.Commands.Update;

public class UpdateCommandValidator : AbstractValidator<UpdateCommand>
{
    public UpdateCommandValidator()
    {
        RuleFor(x => x.Username)
            .Equal(x => x.Hunter.Username)
            .WithMessage("Not entity owner");
        RuleFor(x => x.MangadexId)
            .Equal(x => x.Hunter.MangadexId.ToString())
            .WithMessage("Conflict with mangadexId.");
    }
}