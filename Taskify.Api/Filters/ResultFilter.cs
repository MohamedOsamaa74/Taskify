using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Taskify.Application.ResultPattern;

namespace Taskify.Api.Filters
{
    public class ResultFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult objectResult && objectResult.Value is Application.ResultPattern.IResult result)
            {
                var resultType = result.GetType();
                var statusProperty = resultType.GetProperty("StatusCode");

                if (statusProperty != null)
                {
                    var status = (ResultStatus)statusProperty.GetValue(result);
                    objectResult.StatusCode = MapResultStatusToHttpStatusCode(status);
                }
            }
        }

        private static int MapResultStatusToHttpStatusCode(ResultStatus status)
        {
            return status switch
            {
                ResultStatus.ok => StatusCodes.Status200OK,
                ResultStatus.Created => StatusCodes.Status201Created,
                ResultStatus.BadRequest => StatusCodes.Status400BadRequest,
                ResultStatus.UnAuthorized => StatusCodes.Status401Unauthorized,
                ResultStatus.NotFound => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };
        }
    }
}