using ApiPelicula.Data;
using ApiPelicula.Models;
using ApiPelicula.Models.Dtos;
using ApiPelicula.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using XSystem.Security.Cryptography;

namespace ApiPelicula.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly ApplicationDbContext _context;
        private string _configuration;

        public UsuarioRepositorio(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration.GetValue<string>("ApiSettings:Secreta")!;
        }

        public Usuario GetUsuario(int usuarioId)
        {
            return _context.Usuario.FirstOrDefault(u => u.Id == usuarioId)!;
        }

        public ICollection<Usuario> GetUsuarios()
        {
            return _context.Usuario.OrderBy(u => u.NombreUsuario).ToList();
        }

        public bool IsUniqueUser(string usuario)
        {
            var user = _context.Usuario.FirstOrDefault(u => u.NombreUsuario == usuario);

            if (user == null)
            {
                return true;
            }

            return false;
        }

        public async Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto)
        {
            var passwordEncriptado = obtenermd5(usuarioLoginDto.Password);

            var usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.NombreUsuario.ToLower() == usuarioLoginDto.NombreUsuario.ToLower() && u.Password == passwordEncriptado);

            //Validar que exista

            if (usuario == null)
            {
                return new UsuarioLoginRespuestaDto()
                {
                    Token = "",
                    Usuario = null!
                };
            }

            //Aquí existe el usuario

            var manejadortoken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration);

            var tokenDescripto = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.Nombre.ToString()),
                    new Claim(ClaimTypes.Role, usuario.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = manejadortoken.CreateToken(tokenDescripto);

            UsuarioLoginRespuestaDto usuarioLoginRespuestaDto = new()
            {
                Token = manejadortoken.WriteToken(token),
                Usuario = usuario
                
            };

            return usuarioLoginRespuestaDto;

        }

        public async Task<Usuario> Registro(UsuarioRegistroDto usuarioRegistroDto)
        {
            var passwordEncriptado = obtenermd5(usuarioRegistroDto.Password);

            Usuario usuario = new()
            {
                NombreUsuario = usuarioRegistroDto.NombreUsuario,
                Password = passwordEncriptado,
                Nombre = usuarioRegistroDto.Nombre,
                Role = usuarioRegistroDto.Role
            };

            _context.Usuario.Add(usuario);
            await _context.SaveChangesAsync();
            usuario.Password = passwordEncriptado;
            return usuario;
        }

        public static string obtenermd5(string valor)
        {
            MD5CryptoServiceProvider x = new();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(valor);
            data = x.ComputeHash(data);
            string resp = "";
            for (int i = 0; i < data.Length; i++)
            {
                resp += data[i].ToString("x2").ToLower();
            }
            return resp;
        }
    }
}
