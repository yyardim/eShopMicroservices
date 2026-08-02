using SharedKernel.CQRS;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace SharedKernel.Behaviors;

public class ValidationBehavior<TRequest, TResponse>
    (IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        ValidationContext<TRequest> context = new(request);

        ValidationResult[] valiationResults =
            await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, ct)));

        List<ValidationFailure> failures = [.. valiationResults
            .Where(r => r.Errors.Count != 0)
            .SelectMany(r => r.Errors)];

        if (failures.Count != 0)
            throw new ValidationException(failures);

        return await next(ct);
    }
}
