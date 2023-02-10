using Microsoft.AspNetCore.Mvc;

namespace EmployeeFullStack.Controllers
{
    public class DepartmentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
