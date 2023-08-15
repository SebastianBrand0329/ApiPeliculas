using ApiPelicula.Data;
using ApiPelicula.Models;
using ApiPelicula.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;

namespace ApiPelicula.Repositorio
{
    public class PeliculaRepositorio : IPeliculaRepositorio
    {
        private readonly ApplicationDbContext _context;

        public PeliculaRepositorio(ApplicationDbContext context)
        {
           _context = context;
        }
        public bool ActualizarPelicula(Pelicula pelicula)
        {
            pelicula.FechaCreacion = DateTime.Now;
            _context.Pelicula.Update(pelicula);
            return Guardar();
        }

        public bool BorrarPelicula(Pelicula pelicula)
        {
            _context.Remove(pelicula);
            return Guardar();
        }

        public ICollection<Pelicula> BuscarPelicula(string nombre)
        {
            IQueryable<Pelicula> peliculas = _context.Pelicula;

            if (!string.IsNullOrEmpty(nombre))
            {
                peliculas = peliculas.Where(e => e.Nombre!.Contains(nombre) || e.Descripcion!.Contains(nombre));
            }

            return peliculas.ToList();
        }

        public bool CrearPelicula(Pelicula pelicula)
        {
            pelicula.FechaCreacion = DateTime.Now;
            _context.Pelicula.Add(pelicula);
            return Guardar();
        }

        public bool ExistePelicula(string nombre)
        {
            bool pelicula = _context.Pelicula.Any(p => p.Nombre!.ToLower().Trim() == nombre.ToLower().Trim());
            return pelicula;    
        }

        public bool ExistePelicula(int id)
        {
            bool pelicula = _context.Pelicula.Any(p => p.Id == id); 
            return pelicula;
        }

        public Pelicula GetPelicula(int peliculaId)
        {
            return _context.Pelicula.FirstOrDefault(p => p.Id == peliculaId)!;
        }

        public ICollection<Pelicula> GetPeliculas()
        {
            return _context.Pelicula.OrderBy(p => p.Nombre).ToList();
        }

        public ICollection<Pelicula> GetPeliculasEnCategoria(int categoriaId)
        {
            return _context.Pelicula
                    .Include(ca => ca.Categoria)
                    .Where(ca => ca.categoriaId == categoriaId)
                    .ToList();   
        }

        public bool Guardar()
        {
            return _context.SaveChanges() >= 0 ? true : false ;
        }
    }
}
