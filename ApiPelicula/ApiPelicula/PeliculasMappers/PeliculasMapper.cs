using ApiPelicula.Models;
using ApiPelicula.Models.Dtos;
using AutoMapper;

namespace ApiPelicula.PeliculasMapper
{
    public class PeliculasMapper : Profile
    {
        public PeliculasMapper()
        {
             CreateMap<Categoria, CategoriaDto>().ReverseMap();
            CreateMap<Categoria, CategoriaCreacionDto>().ReverseMap();   
        }
    }
}
