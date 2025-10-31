using Microsoft.AspNetCore.Mvc;

namespace CS_ClothesStore.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult ErrorPage()
        {
            return View();
        }
    }
}
