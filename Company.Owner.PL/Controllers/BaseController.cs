using Microsoft.AspNetCore.Mvc;

namespace Company.Owner.PL.Controllers
{
    public class BaseController : Controller
    {
        public class BaseSearchController : Controller
        {
            protected async Task<IActionResult> GetSearchResult<T>(string searchInput,
                Func<string, Task<IEnumerable<T>>> searchFunc,
                string partialViewPath)
            {
                IEnumerable<T> results;
                if (string.IsNullOrEmpty(searchInput))
                {
                    results = await searchFunc("");
                }
                else
                {
                    results = await searchFunc(searchInput);
                }

                if (results.Any())
                {
                    return PartialView(partialViewPath, results);
                }

                return Content("<tr><td colspan='15'>No records found</td></tr>");
            }
        }
    }
}
