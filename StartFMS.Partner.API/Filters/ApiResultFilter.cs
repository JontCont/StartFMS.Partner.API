using Microsoft.AspNetCore.Mvc.Filters;

namespace StartFMS.Partner.API.Filters
{
    public class ApiResultFilter : IResultFilter
    {
        public void OnResultExecuting(ResultExecutingContext context)
        {
            // Do something before the action executes.
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            // Do something after the action executes.
        }
    }
}
