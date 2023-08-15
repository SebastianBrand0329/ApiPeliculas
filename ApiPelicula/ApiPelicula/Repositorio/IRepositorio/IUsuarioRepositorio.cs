using ApiPelicula.Models;
using ApiPelicula.Models.Dtos;

namespace ApiPelicula.Repositorio.IRepositorio
{
    public interface IUsuarioRepositorio
    {
        ICollection<Usuario> GetUsuarios();

        Usuario GetUsuario(int usuarioId);

        bool IsUniqueUser(string usuario);

        Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto);

        Task<Usuario> Registro(UsuarioRegistroDto usuarioRegistroDto);


    }
}
