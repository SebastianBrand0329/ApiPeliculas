using ApiPelicula.Models;
using ApiPelicula.Models.Dtos;
using ApiPelicula.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApiPelicula.Controllers
{
    [ApiController]
    [Route("api/categorias")]    
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaRepositorio _repositorio;
        private readonly IMapper _mapper;

        public CategoriasController(ICategoriaRepositorio repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCategoria()
        {
            var lista = _repositorio.GetCategorias();

            var listaCategoriaDto = new List<CategoriaDto>();

            foreach (var item in lista)
            {
                listaCategoriaDto.Add(_mapper.Map<CategoriaDto>(item)); 
            }

            return Ok(listaCategoriaDto);
        }


        [HttpGet("categoriaId:int", Name ="GetCategoria")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCategoria(int categoriaId)
        {
            var categoria = _repositorio.GetCategoria(categoriaId);

            if(categoria == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CategoriaDto>(categoria));

        }

        [HttpPost]
        public IActionResult CreateCategoria([FromBody] CategoriaCreacionDto categoriaCreacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(categoriaCreacion is null)
            {
                return BadRequest(ModelState);
            }

            if (_repositorio.ExisteCategoria(categoriaCreacion.Nombre!))
            {
                ModelState.AddModelError("", "La categoria ya existe");
                return StatusCode(404, ModelState);
            }

            var categoria = _mapper.Map<Categoria>(categoriaCreacion);
            if (!_repositorio.CrearCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salió mal guardando el registro {categoria.Nombre}");
                return StatusCode(404, ModelState);
            }
            return CreatedAtRoute("GetCategoria", new { categoriaId = categoria.Id }, categoria);
        }

        [HttpPatch("{categoriaId:int}", Name = "ActualizarPatchCategoria")]
        public IActionResult ActualizarPatchCategoria(int categoriaId, [FromBody] CategoriaDto categoriaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (categoriaDto == null || categoriaDto.Id != categoriaId)
            {
                return BadRequest(ModelState);
            }

            var categoria = _mapper.Map<Categoria>(categoriaDto);
            if (!_repositorio.ActualizarCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salió mal actualizando el registro {categoria.Nombre}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{categoriaId:int}", Name = "EliminarCategoria")]
        public IActionResult EliminarCategoria(int categoriaId)
        {
            if (!_repositorio.ExisteCategoria(categoriaId))
            {
                return NotFound();  
            }

            var categoria = _repositorio.GetCategoria(categoriaId);

            if (!_repositorio.BorrarCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salió mal eliminando el registro {categoria.Nombre}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }


    }
}
