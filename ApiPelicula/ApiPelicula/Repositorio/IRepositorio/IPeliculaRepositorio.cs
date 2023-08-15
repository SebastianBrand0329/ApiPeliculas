using ApiPelicula.Models;

namespace ApiPelicula.Repositorio.IRepositorio
{
    public interface IPeliculaRepositorio
    {
        ICollection<Pelicula> GetPeliculas();
        
        Pelicula GetPelicula(int peliculaId);

        bool ExistePelicula(string nombre);

        bool ExistePelicula(int id);

        bool CrearPelicula(Pelicula pelicula);

        bool ActualizarPelicula(Pelicula pelicula);

        bool BorrarPelicula(Pelicula pelicula);

        //Métodos para buscar películas en categoría y buscar película por nombre

        ICollection<Pelicula> GetPeliculasEnCategoria(int categoriaId);

        ICollection<Pelicula> BuscarPelicula(string nombre);

        bool Guardar();
    }

}

