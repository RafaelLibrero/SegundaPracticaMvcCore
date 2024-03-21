using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SegundaPracticaMvcCore.Models
{
    [Table("GENEROS")]
    public class Genero
    {
        [Key]
        [Column("IdGenero")]
        public int GeneroId { get; set; }
        [Column("Nombre")]
        public string Nombre { get; set; }
    }
}
