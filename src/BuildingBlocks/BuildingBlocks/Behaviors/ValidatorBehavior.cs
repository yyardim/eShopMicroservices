using BuildingBlocks.CQRS;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace BuildingBlocks.Behaviors;

public class ValidatorBehavior<TRequest, TResponse>
    (IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        ValidationContext<TRequest> context = new(request);

        ValidationResult[] valiationResults = await Task.WhenAll(
            validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        List<ValidationFailure> failures = [.. valiationResults
            .Where(r => r.Errors.Count != 0)
            .SelectMany(r => r.Errors)];

        if (failures.Count != 0) throw new ValidationException(failures);

        return await next(cancellationToken);
    }
}
