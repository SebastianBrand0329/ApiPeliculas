using Microsoft.AspNetCore.Identity;

namespace ApiPelicula.Models
{
    public class AppUsuario : IdentityUser
    {
        //añadir campos personalizados

        public string Nombre { get; set; }
    }
}
