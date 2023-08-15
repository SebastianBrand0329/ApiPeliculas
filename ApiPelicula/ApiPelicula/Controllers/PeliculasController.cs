using ApiPelicula.Models;
using ApiPelicula.Models.Dtos;
using ApiPelicula.Repositorio.IRepositorio;
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Mvc;

namespace ApiPelicula.Controllers
{
    [ApiController]
    [Route("api/peliculas")]
    public class PeliculasController : ControllerBase
    {
        private readonly IPeliculaRepositorio _repositorio;
        private readonly IMapper _mapper;

        public PeliculasController(IPeliculaRepositorio repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetPeliculas()
        {
            var listaPeliculas = _repositorio.GetPeliculas();
            var listaPeliculasDto = new List<PeliculaDto>();
            foreach (var item in listaPeliculas)
            {
                listaPeliculasDto.Add(_mapper.Map<PeliculaDto>(item));
            }

            return Ok(listaPeliculasDto);
        }

        [HttpGet("{peliculaId:int}", Name = "GetPelicula")]
        public IActionResult GetPelicula(int peliculaId)
        {
            var pelicula = _repositorio.GetPelicula(peliculaId);
            if (pelicula == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PeliculaDto>(pelicula));
        }

        [HttpPost]
        public IActionResult CrearPelicula([FromBody] PeliculaDto peliculaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (peliculaDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_repositorio.ExistePelicula(peliculaDto.Nombre!))
            {
                ModelState.AddModelError("", "La película la existe");
                return StatusCode(500, ModelState);
            }


            var pelicula = _mapper.Map<Pelicula>(peliculaDto);
            if (!_repositorio.CrearPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salió mal guardando el registro{pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetPelicula", new { peliculaId = pelicula.Id }, pelicula);
        }

        [HttpPatch("{peliculaId:int}", Name = "ActualizarPatchPelicula")]
        public IActionResult ActualizarPatchPelicula(int peliculaId, [FromBody] PeliculaDto peliculaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pelicula = _mapper.Map<Pelicula>(peliculaDto);
            if (!_repositorio.ActualizarPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salió mal actualizando el registro {pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{peliculaId:int}", Name = "BorrarPelicula")]
        public IActionResult BorrarPelicula(int peliculaId)
        {
            if (!_repositorio.ExistePelicula(peliculaId))
            {
                return NotFound();
            }

            var pelicula = _repositorio.GetPelicula(peliculaId);

            if (!_repositorio.BorrarPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salió mal eliminando el registro{pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


        [HttpGet("GetPeliculasEnCategoria/{categoriaId:int}")]
        public IActionResult GetPeliculasEnCategoria(int categoriaId)
        {
            var listaPeliculas = _repositorio.GetPeliculasEnCategoria(categoriaId);

            if(listaPeliculas == null)
            {
                return NotFound();
            }

            var itemPelicula = new List<PeliculaDto>();
            foreach (var item in listaPeliculas)
            {
                itemPelicula.Add(_mapper.Map<PeliculaDto>(item));
            }

            return Ok(itemPelicula);
        }

        [HttpGet("Buscar")]
        public IActionResult Buscar(string nombre)
        {
            try
            {
                var resultado = _repositorio.BuscarPelicula(nombre.Trim());
                if (resultado.Any())
                {
                    return Ok(resultado);
                }
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando datos");
            }
           
        }


    }
}
