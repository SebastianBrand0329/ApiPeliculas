using System.ComponentModel.DataAnnotations;

namespace ApiPelicula.Models.Dtos
{
    public class CategoriaDto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El Campo {0} es obligatorio")]
        [MaxLength(60, ErrorMessage = "El número máximo de caracteres es de {1}")]
        public string? Nombre { get; set; }

    }
}
