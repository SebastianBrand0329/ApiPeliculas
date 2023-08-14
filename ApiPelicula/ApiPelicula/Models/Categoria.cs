using System.ComponentModel.DataAnnotations;

namespace ApiPelicula.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Nombre { get; set; }

        public DateTime FechaCreacion { get; set; }
    }
}
