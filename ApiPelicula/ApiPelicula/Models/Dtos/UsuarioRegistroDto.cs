using System.ComponentModel.DataAnnotations;

namespace ApiPelicula.Models.Dtos
{
    public class UsuarioRegistroDto
    {
        [Required(ErrorMessage ="El campo {0} es obligatorio")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Password { get; set; }

        public string Role { get; set; }
    }
}
