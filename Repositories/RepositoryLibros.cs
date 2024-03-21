using Microsoft.EntityFrameworkCore;
using SegundaPracticaMvcCore.Data;
using SegundaPracticaMvcCore.Models;

namespace SegundaPracticaMvcCore.Repositories
{
    public class RepositoryLibros
    {
        private LibrosContext context;

        public RepositoryLibros(LibrosContext context)
        {
            this.context = context;
        }

        public async Task<List<Libro>> GetLibrosAsync()
        {
            return await
                this.context.Libros.ToListAsync();
        }

        public async Task<Libro> FindLibroAsync(int libroId)
        {
            return await
                this.context.Libros.FirstOrDefaultAsync
                (x => x.LibroId == libroId);
        }

        public async Task<List<Libro>> GetLibrosGeneroAsync(int generoId)
        {
            return await
                this.context.Libros
                .Where(x => x.GeneroId == generoId).ToListAsync();
        }

        public async Task<List<Libro>> GetLibrosCarroAsync(List<int> ids)
        {
            return await 
                this.context.Libros
                .Where(x => ids.Contains(x.LibroId)).ToListAsync();
        }

        public async Task<Usuario> LoginUsuarioAsync (string email, string password)
        {
            Usuario usuario = await this.context.Usuarios.FirstOrDefaultAsync
                (x => x.Email == email);

            if (usuario == null)
            {
                return null;
            }
            else
            {
                if (usuario.Pass == password)
                {
                    return usuario;
                }

                else
                {
                    return null;
                }
            }
        }

        public async Task<List<Genero>> GetGenerosAsync()
        {
            return await this.context.Generos.ToListAsync();
        }

        public async Task ComprarAsync(List<int> ids, int usuarioId)
        {
            int facturaId = await this.context.Pedidos.MaxAsync(x => x.FacturaId) + 1;
            DateTime fecha = DateTime.Now;
            foreach (int id in ids) {
                await this.InsertPedido(facturaId, fecha, id, usuarioId);
            }
        }

        public async Task InsertPedido (int facturaId, DateTime fecha,  int libroId, int usuarioId)
        {
            Pedido pedido = new Pedido();

            pedido.PedidoId = await this.context.Pedidos.MaxAsync(x => x.PedidoId) + 1;
            pedido.FacturaId = facturaId;
            pedido.Fecha = fecha;
            pedido.LibroId = libroId;
            pedido.UsuarioId = usuarioId;
            pedido.Cantidad = 1;

            this.context.Pedidos.Add(pedido);
            await this.context.SaveChangesAsync();
        }

        public async Task<List<VistaPedido>> GetVistaPedidosUsuarioAsync (int usuarioId)
        {
            return await
                this.context.VistaPedidos
                .Where(x => x.UsuarioId == usuarioId).ToListAsync();
        }
    }
}
