using FluentValidation;
using MediatR;
using ServiceMonitor.Domain.Exceptions;

namespace ServiceMonitor.API.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!validators.Any())
            return await next(cancellationToken);

        var context = new ValidationContext<TRequest>(request);

        var errors = validators
            .Select(x => x.Validate(context))
            .SelectMany(x => x.Errors)
            .Where(x => x is not null)
            .GroupBy(
                x => x.PropertyName.Substring(x.PropertyName.IndexOf('.') + 1),
                x => x.ErrorMessage,
                (propertyName, errorMessages) =>
                    new { Key = propertyName, Value = errorMessages.Distinct().ToArray() })
            .ToDictionary(x => x.Key, x => x.Value);

        if (errors.Any())
        {
            throw new ValidationAppException(errors);
        }

        return await next();
    }
}
