using Dqsm.Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace Dqsm.Backend.Filters
{
    /// <summary>
    /// Global Audit Log Filter (AOP)
    /// Requirement: "在每个action入参时需要记录参数值、操作结果、返回值，均需记录日志"
    /// </summary>
    public class GlobalAuditLogFilter : IActionFilter
    {
        private readonly ILogger<GlobalAuditLogFilter> _logger;

        public GlobalAuditLogFilter(ILogger<GlobalAuditLogFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Capture Action Name
            var actionName = $"{context.RouteData.Values["controller"]}/{context.RouteData.Values["action"]}";
            
            // Capture Parameters
            var parameters = JsonConvert.SerializeObject(context.ActionArguments);

            // Log Input
            StaticLogService.Info(actionName, $"[Input] Parameters: {parameters}");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Capture Action Name
            var actionName = $"{context.RouteData.Values["controller"]}/{context.RouteData.Values["action"]}";

            if (context.Exception != null)
            {
                // Log Exception/Error
                StaticLogService.Error(actionName, $"[Exception] {context.Exception.Message}");
            }
            else
            {
                // Capture Result/Return Value
                string resultJson = "None";
                if (context.Result is ObjectResult objectResult)
                {
                    resultJson = JsonConvert.SerializeObject(objectResult.Value);
                }
                else if (context.Result is JsonResult jsonResult)
                {
                    resultJson = JsonConvert.SerializeObject(jsonResult.Value);
                }
                else if (context.Result is ViewResult viewResult)
                {
                    resultJson = $"[ViewResult] ViewName: {viewResult.ViewName}, Model: {viewResult.Model?.GetType().Name}";
                }
                else if (context.Result is ContentResult contentResult)
                {
                    resultJson = contentResult.Content ?? "Empty Content";
                }

                // Log Output
                StaticLogService.Info(actionName, $"[Output] Result: {resultJson}");
            }
        }
    }
}
