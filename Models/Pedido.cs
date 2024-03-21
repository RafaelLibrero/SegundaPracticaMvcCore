using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SegundaPracticaMvcCore.Models
{
    [Table("PEDIDOS")]
    public class Pedido
    {
        [Key]
        [Column("IDPEDIDO")]
        public int? PedidoId { get; set; }
        [Column("IDFACTURA")]
        public int FacturaId { get; set; }
        [Column("FECHA")]
        public DateTime Fecha { get; set; }
        [Column("IDLIBRO")]
        public int LibroId { get; set; }
        [Column("IDUSUARIO")]
        public int UsuarioId { get; set; }
        [Column("CANTIDAD")]
        public int Cantidad { get; set; }
    }
}
