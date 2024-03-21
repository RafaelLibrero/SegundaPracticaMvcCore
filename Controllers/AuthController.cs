using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SegundaPracticaMvcCore.Models;
using SegundaPracticaMvcCore.Repositories;
using System.Security.Claims;

namespace SegundaPracticaMvcCore.Controllers
{
    public class AuthController : Controller
    {
        private RepositoryLibros repo;

        public AuthController(RepositoryLibros repo)
        {
            this.repo = repo;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            Usuario usuario = await this.repo.LoginUsuarioAsync(email, password);

            if (usuario != null) 
            {
                ClaimsIdentity identity =
                    new ClaimsIdentity(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        ClaimTypes.Name, ClaimTypes.NameIdentifier);

                Claim claimUsuarioId =
                    new Claim(ClaimTypes.NameIdentifier, usuario.UsuarioId.ToString());
                identity.AddClaim(claimUsuarioId);
                Claim claimName = 
                    new Claim(ClaimTypes.Name, usuario.Nombre);
                identity.AddClaim(claimName);
                Claim claimApellido =
                    new Claim("Apellido", usuario.Apellidos);
                identity.AddClaim(claimApellido);
                Claim claimEmail = 
                    new Claim(ClaimTypes.Email, usuario.Email);
                identity.AddClaim(claimEmail);
                Claim claimImagen =
                    new Claim("Imagen", usuario.Foto);
                identity.AddClaim(claimImagen);

                ClaimsPrincipal userPrincipal =
                    new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    userPrincipal);

                return RedirectToAction("Perfil", "Usuarios");
            }
            else
            {
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync
                (CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("IDSLIBROS");
            return RedirectToAction("Index", "Libros");
        }
    }
}
