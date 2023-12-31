﻿using ApiPelicula.Models;
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
            
            CreateMap<Pelicula, PeliculaDto>().ReverseMap();  
            //CreateMap<Pelicula>

            //CreateMap<Usuario, UsuarioDto>().ReverseMap();

            CreateMap<AppUsuario, UsuarioDto>().ReverseMap();

            CreateMap<AppUsuario, UsuarioDatosDto>().ReverseMap();
        }
    }
}
