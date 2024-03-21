using Microsoft.AspNetCore.Mvc;
using SegundaPracticaMvcCore.Filters;

namespace SegundaPracticaMvcCore.Controllers
{
    public class UsuariosController : Controller
    {
        [AuthorizeUsers]
        public IActionResult Perfil()
        {
            return View();
        }
    }
}
