﻿using System.ComponentModel.DataAnnotations;

namespace ApiPelicula.Models.Dtos
{
    public class UsuarioDto
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string UserName { get; set; }

        public string Nombre { get; set; }

        public string Password { get; set; }

        public DateTime FechaCreacion { get; set; }

        public string Role { get; set; }
    }
}
