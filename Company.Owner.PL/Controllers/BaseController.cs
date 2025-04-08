using Microsoft.AspNetCore.Mvc;

namespace Company.Owner.PL.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly ILogger<BaseController> _logger;

        protected BaseController(ILogger<BaseController> logger)
        {
            _logger = logger;
        }

        protected async Task<IActionResult> PerformSearch<T>(
            string searchInput,
            Func<string, Task<IEnumerable<T>>> emptySearchFunc,
            Func<string, Task<IEnumerable<T>>> filteredSearchFunc,
            string partialViewName)
        {
            try
            {
                IEnumerable<T> results;

                if (string.IsNullOrWhiteSpace(searchInput))
                {
                    results = await emptySearchFunc("");
                }
                else
                {
                    results = await filteredSearchFunc(searchInput);
                }

                if (results == null || !results.Any())
                {
                    return Content("<tr><td colspan='15'>No records found</td></tr>");
                }

                return PartialView(partialViewName, results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Search operation failed");
                return Content("<tr><td colspan='15'>Error during search operation</td></tr>");
            }
        }
    }
}
