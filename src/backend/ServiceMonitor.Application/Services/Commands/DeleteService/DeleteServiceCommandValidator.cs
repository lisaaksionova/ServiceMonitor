using FluentValidation;

namespace ServiceMonitor.Application.Services.Commands.DeleteService;

public class DeleteServiceCommandValidator : AbstractValidator<DeleteServiceCommand>
{
    public DeleteServiceCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .WithMessage("ServiceId is required.");
    }
}
