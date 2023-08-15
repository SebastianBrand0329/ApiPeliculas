using ApiPelicula.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiPelicula.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        //Agregar los modelos aquí

        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Pelicula> Pelicula { get; set; }
    }
}
