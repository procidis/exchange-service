using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ExchangeService.Api.Filters;

public class ValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.ModelState.IsValid)
        {
            var errorInModelState = context.ModelState.Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.First()
                );

            throw new ValidationException(
                    errorInModelState.Select(w => new ValidationFailure
                    {
                        PropertyName = w.Key,
                        ErrorMessage = w.Value.ErrorMessage
                    }))
                ;
        }

        await next();
    }
}