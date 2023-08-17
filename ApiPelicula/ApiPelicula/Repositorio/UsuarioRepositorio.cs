using ApiPelicula.Data;
using ApiPelicula.Models;
using ApiPelicula.Models.Dtos;
using ApiPelicula.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<AppUsuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private string _configuration;

        public UsuarioRepositorio(ApplicationDbContext context, IConfiguration configuration, UserManager<AppUsuario> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _configuration = configuration.GetValue<string>("ApiSettings:Secreta")!;
        }

        public AppUsuario GetUsuario(string usuarioId)
        {
            return _context.appUsuarios.FirstOrDefault(u => u.Id == usuarioId);
        }

        public ICollection<AppUsuario> GetUsuarios()
        {
            return _context.appUsuarios.OrderBy(u => u.UserName).ToList();
        }

        public bool IsUniqueUser(string usuario)
        {
            var user = _context.appUsuarios.FirstOrDefault(u => u.UserName == usuario);

            if (user == null)
            {
                return true;
            }

            return false;
        }

        public async Task<UsuarioLoginRespuestaDto> Login(UsuarioLoginDto usuarioLoginDto)
        {
            //var passwordEncriptado = obtenermd5(usuarioLoginDto.Password);

            var usuario =  _context.appUsuarios.FirstOrDefault(u => u.UserName!.ToLower() == usuarioLoginDto.NombreUsuario.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(usuario!, usuarioLoginDto.Password);

            //Validar que exista

            if (usuario == null || isValid == false)
            {
                return new UsuarioLoginRespuestaDto()
                {
                    Token = "",
                    Usuario = null!
                };
            }

            //Aquí existe el usuario

            var roles = await _userManager.GetRolesAsync(usuario);

            var manejadortoken = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration);

            var tokenDescripto = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.UserName.ToString()),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = manejadortoken.CreateToken(tokenDescripto);

            UsuarioLoginRespuestaDto usuarioLoginRespuestaDto = new()
            {
                Token = manejadortoken.WriteToken(token),
                Usuario = _mapper.Map<UsuarioDatosDto>(usuario)

            };

            return usuarioLoginRespuestaDto;

        }

        public async Task<UsuarioDatosDto> Registro(UsuarioRegistroDto usuarioRegistroDto)
        {
            //var passwordEncriptado = obtenermd5(usuarioRegistroDto.Password);

            AppUsuario usuario = new AppUsuario()
            {
                UserName = usuarioRegistroDto.NombreUsuario,
                Email = usuarioRegistroDto.NombreUsuario,
                NormalizedEmail = usuarioRegistroDto.NombreUsuario.ToUpper(),
                Nombre = usuarioRegistroDto.Nombre
            };

            var result = await _userManager.CreateAsync(usuario, usuarioRegistroDto.Password);
            if (result.Succeeded)
            {
                //Solo para la crecion de roles, la primera vez
                if (!_roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
                {
                    await _roleManager.CreateAsync(new IdentityRole("admin"));
                    await _roleManager.CreateAsync(new IdentityRole("registrado"));
                }

                await _userManager.AddToRoleAsync(usuario, "admin");
                var usuarioRetornado = _context.appUsuarios.FirstOrDefault(u => u.UserName == usuarioRegistroDto.NombreUsuario);

                //opción 1
                //return new UsuarioDatosDto()
                //{
                //    Id = usuarioRetornado.Id,
                //    UserName = usuarioRetornado.UserName,
                //    Nombre = usuarioRetornado.Nombre

                //};

                return _mapper.Map<UsuarioDatosDto>(usuarioRetornado);
            }

            return new UsuarioDatosDto();

            //_context.Usuario.Add(usuario);
            //await _context.SaveChangesAsync();
            //usuario.Password = passwordEncriptado;
            //return usuario;
        }

        //public static string obtenermd5(string valor)
        //{
        //    MD5CryptoServiceProvider x = new();
        //    byte[] data = System.Text.Encoding.UTF8.GetBytes(valor);
        //    data = x.ComputeHash(data);
        //    string resp = "";
        //    for (int i = 0; i < data.Length; i++)
        //    {
        //        resp += data[i].ToString("x2").ToLower();
        //    }
        //    return resp;
        //}
    }
}
