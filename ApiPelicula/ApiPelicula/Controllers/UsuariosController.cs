using ApiPelicula.Models;
using ApiPelicula.Models.Dtos;
using ApiPelicula.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ApiPelicula.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioRepositorio _repositorio;
        private readonly IMapper _mapper;
        protected RespuestaApi _respuestaApi;

        public UsuariosController(IUsuarioRepositorio repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _respuestaApi = new();
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetUsuarios()
        {
            var lista = _repositorio.GetUsuarios();

            var listaUsuariosDto = new List<UsuarioDto>();

            foreach (var item in lista)
            {
                listaUsuariosDto.Add(_mapper.Map<UsuarioDto>(item));
            }

            return Ok(listaUsuariosDto);
        }

        [HttpGet("usuarioId:int", Name = "GetUsuario")]
        public IActionResult GetUsuario(int usuarioId)
        {
            var itmUsuario = _repositorio.GetUsuario(usuarioId);

            if(itmUsuario == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<UsuarioDto>(itmUsuario));
        }

        [HttpPost (Name ="Registro")]
        public async Task<IActionResult> Registro([FromBody] UsuarioRegistroDto usuarioRegistroDto)
        {
            bool validarNombre = _repositorio.IsUniqueUser(usuarioRegistroDto.NombreUsuario);

            if (!validarNombre)
            {
                _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.ErrorMessages.Add("Error, el nombre de usuario ya existe");
                return BadRequest(_respuestaApi);
            }

            var usuario = await _repositorio.Registro(usuarioRegistroDto);

            if(usuario == null)
            {
                _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.ErrorMessages.Add("Error, generando el registro de usuario");
                return BadRequest(_respuestaApi);
            }
            _respuestaApi.StatusCode = HttpStatusCode.OK;
            _respuestaApi.IsSuccess = true;
            return Ok(_respuestaApi);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginDto usuarioLoginDto)
        {
            var respuesta = await _repositorio.Login(usuarioLoginDto);

            if (respuesta.Usuario == null || string.IsNullOrEmpty(respuesta.Token))
            {
                _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.ErrorMessages.Add("Error, el nombre de usuario o contraseña no es correcto");
                return BadRequest(_respuestaApi);
            }

            _respuestaApi.StatusCode = HttpStatusCode.OK;
            _respuestaApi.IsSuccess = true;
            _respuestaApi.Result = respuesta;
            return Ok(_respuestaApi);
        }


    }
}
