using FluentValidation;

namespace ServiceMonitor.Application.History.Queries.HourlyServiceCheck.GetAll;

public class GetAllHourlyServiceChecksQueryValidator : AbstractValidator<GetAllHourlyServiceChecksQuery>
{
    public GetAllHourlyServiceChecksQueryValidator()
    {
        RuleFor(g => g.From)
            .NotNull()
            .WithMessage("FromDate cannot be null")
            .LessThan(g => g.To);
        RuleFor(g => g.To)
            .NotNull()
            .WithMessage("ToDate cannot be null")
            .GreaterThan(g => g.From);
        RuleFor(g => g.To - g.From)
            .LessThan(TimeSpan.FromDays(31));
    }
}
