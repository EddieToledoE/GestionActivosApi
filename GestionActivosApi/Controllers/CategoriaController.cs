using GestionActivos.Application.CategoriaApplication.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GestionActivos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriaController : ControllerBase
    {
        private readonly IMediator _mediator;

  public CategoriaController(IMediator mediator)
  {
       _mediator = mediator;
      }

        [HttpGet]
  public async Task<IActionResult> GetAll()
        {
  var categorias = await _mediator.Send(new GetCategoriasQuery());
         return Ok(categorias);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
     var categoria = await _mediator.Send(new GetCategoriaByIdQuery(id));
     return Ok(categoria);
      }
    }
}
