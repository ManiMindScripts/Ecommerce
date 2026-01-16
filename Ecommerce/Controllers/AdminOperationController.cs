using Ecommerce.Constrants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [Authorize(Roles =nameof(Roles.Admin))]
    public class AdminOperationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
