using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging.Signing;
using SegundaPracticaMvcCore.Extensions;
using SegundaPracticaMvcCore.Filters;
using SegundaPracticaMvcCore.Helpers;
using SegundaPracticaMvcCore.Models;
using SegundaPracticaMvcCore.Repositories;
using System.Security.Claims;

namespace SegundaPracticaMvcCore.Controllers
{
    public class LibrosController : Controller
    {

        private RepositoryLibros repo;
        private HelperPathProvider helperPathProvider;

        public LibrosController(RepositoryLibros repo, HelperPathProvider helperPathProvider)
        {
            this.repo = repo;
            this.helperPathProvider = helperPathProvider;
        }

        public async Task<IActionResult> Index()
        {
            List<Libro> libros = await this.repo.GetLibrosAsync();
            foreach (Libro libro in libros)
            {
                libro.Portada = this.helperPathProvider.MapUrlPath(libro.Portada);
            }
            return View(libros);
        }

        public async Task<IActionResult> IndexGenero(int generoId)
        {
            List<Libro> libros = await this.repo.GetLibrosGeneroAsync(generoId);
            foreach (Libro libro in libros)
            {
                libro.Portada = this.helperPathProvider.MapUrlPath(libro.Portada);
            }
            return View(libros);
        }

        public async Task<IActionResult> Details(int libroId)
        {
            Libro libro = await this.repo.FindLibroAsync(libroId);
            libro.Portada = this.helperPathProvider.MapUrlPath(libro.Portada);
            return View(libro);
        }

        public IActionResult AñadirLibro(int libroId)
        {
                List<int> ids;
                if (HttpContext.Session.GetObject<List<int>>("IDSLIBROS") == null)
                    ids = new List<int>();
                else
                    ids = HttpContext.Session.GetObject<List<int>>("IDSLIBROS");
                    ids.Add(libroId);
                    HttpContext.Session.SetObject("IDSLIBROS", ids);

            return RedirectToAction("CarroCompra");
        }

        public IActionResult QuitarLibro(int libroId)
        {
            List<int> ids = HttpContext.Session.GetObject<List<int>>("IDSLIBROS");
            ids.Remove(libroId);
            if (ids.Count == 0)
            {
                HttpContext.Session.Remove("IDSLIBROS");
            } else
            {
                HttpContext.Session.SetObject("IDSLIBROS", ids);
            }
            
            return RedirectToAction("CarroCompra");
        }

        public async Task<IActionResult> CarroCompra()
        {
            List<int> ids = HttpContext.Session.GetObject<List<int>>("IDSLIBROS");

            if (ids != null)
            {
                List<Libro> libros = await this.repo.GetLibrosCarroAsync(ids);
                foreach (Libro libro in libros)
                {
                    libro.Portada = this.helperPathProvider.MapUrlPath(libro.Portada);
                }
                return View(libros);
            }

            return View();
        }

        [AuthorizeUsers]
        public async Task<IActionResult> Comprar()
        {
            List<int> ids = HttpContext.Session.GetObject<List<int>>("IDSLIBROS");
            int usuarioId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            await this.repo.ComprarAsync(ids, usuarioId);
            HttpContext.Session.Remove("IDSLIBROS");
            return RedirectToAction("Pedidos");
        }

        [AuthorizeUsers]
        public async Task<IActionResult> Pedidos()
        {
            int usuarioId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            List<VistaPedido> vistaPedidos = await this.repo.GetVistaPedidosUsuarioAsync(usuarioId);
            return View(vistaPedidos);
        }
    }
}
