﻿using ApiPelicula.Models;
using ApiPelicula.Models.Dtos;

namespace ApiPelicula.Repositorio.IRepositorio
{
    public interface IUsuarioRepositorio
    {
        ICollection<AppUsuario> GetUsuarios();

        AppUsuario GetUsuario(string usuarioId);

        bool IsUniqueUser(string usuario);

        //Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto);

        Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto);

        Task<UsuarioDatosDto> Registro(UsuarioRegistroDto usuarioRegistroDto);


    }
}
