using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace OnlineStore.Domain.Behaviors;
public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)

    {
        ValidationContext<TRequest> context = new(request);

        List<ValidationFailure> failures = _validators
            .ToAsyncEnumerable()
            .SelectAwait(async validator => await validator.ValidateAsync(context))
            .ToEnumerable()
            .SelectMany(result => result.Errors)
            .Where(failure => failure is not null)
            .ToList();

        if (failures.Count != 0)
        {
            throw new ValidationException(failures);
        }

        return await next();
    }
}
