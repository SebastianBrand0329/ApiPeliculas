using ApiPelicula.Data;
using ApiPelicula.Models;
using ApiPelicula.Repositorio.IRepositorio;

namespace ApiPelicula.Repositorio
{
    public class CategoriaRepositorio : ICategoriaRepositorio
    {
        private readonly ApplicationDbContext _context;

        public CategoriaRepositorio(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool ActualizarCategoria(Categoria categoria)
        {
            categoria.FechaCreacion = DateTime.Now;
            _context.Categoria.Update(categoria);
            return Guardar();
        }

        public bool BorrarCategoria(Categoria categoria)
        {
           _context.Remove(categoria);
            return Guardar();
        }

        public bool CrearCategoria(Categoria categoria)
        {
            categoria.FechaCreacion = DateTime.Now;
            _context.Categoria.Add(categoria);
            return Guardar();
        }

        public bool ExisteCategoria(string nombre)
        {
            bool existe = _context.Categoria.Any(c => c.Nombre!.ToLower().Trim() == nombre.ToLower().Trim());
            return existe;
        }

        public bool ExisteCategoria(int id)
        {
            return _context.Categoria.Any(c => c.Id == id);
        }

        public Categoria GetCategoria(int categoriaId)
        {
            return _context.Categoria.FirstOrDefault(c => c.Id == categoriaId)!;
        }

        public ICollection<Categoria> GetCategorias()
        {
            return _context.Categoria.OrderBy(c => c.Nombre).ToList();   
        }

        public bool Guardar()
        {
            return _context.SaveChanges() >= 0 ? true : false;
        }
    }
}
