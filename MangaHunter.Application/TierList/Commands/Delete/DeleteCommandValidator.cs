using FluentValidation;

namespace MangaHunter.Application.TierList.Commands.Delete;

public class DeleteCommandValidator : AbstractValidator<DeleteCommand>
{
    public DeleteCommandValidator()
    {
        // RuleFor(x => x.Username)
        //     .Equal(x => x.Username)
        //     .WithMessage("Not entity owner");
        // RuleFor(x => x.Id)
        //     .Equal(x => x.Id.ToString())
        //     .WithMessage("Conflict with mangadexId");
    }
}