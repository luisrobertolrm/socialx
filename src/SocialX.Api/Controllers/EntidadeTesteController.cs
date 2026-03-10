using Microsoft.AspNetCore.Mvc;
using SocialX.Core.Paginacao;
using SocialX.Service.DTOs;
using SocialX.Service.Interfaces;

namespace SocialX.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EntidadeTesteController : ControllerBase
{
    private readonly IEntidadeTesteService entidadeTesteService;

    public EntidadeTesteController(IEntidadeTesteService entidadeTesteService)
    {
        this.entidadeTesteService = entidadeTesteService;
    }

    [HttpPost]
    public async Task<ActionResult<EntidadeTesteDto>> Post([FromBody] EntidadeTesteCriarDto dto)
    {
        EntidadeTesteDto resultado = await entidadeTesteService.CriarAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = resultado.Id }, resultado);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EntidadeTesteDto>> Get(long id)
    {
        EntidadeTesteDto? entidadeTeste = await entidadeTesteService.ObterPorIdAsync(id);

        if (entidadeTeste == null)
        {
            return NotFound();
        }

        return Ok(entidadeTeste);
    }

    [HttpPost("scroll")]
    public async Task<ActionResult<ListaScrollInfinito<EntidadeTesteDto>>> Scroll(
        [FromBody] ParametrosScrollInfinito parametros,
        CancellationToken cancellationToken)
    {
        ListaScrollInfinito<EntidadeTesteDto> resultado = await entidadeTesteService.ScrollAsync(parametros, cancellationToken);
        return Ok(resultado);
    }
}
