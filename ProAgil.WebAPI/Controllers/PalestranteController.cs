using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain;
using ProAgil.Repository;

namespace ProAgil.WebAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class PalestranteController : ControllerBase
  {
    private readonly IProAgilRepository _repo;
    public PalestranteController(IProAgilRepository repo)
    {
      _repo = repo;
    }

    [HttpGet("{PalestranteId}")]
    public async Task<IActionResult> Get(int PalestranteId)
    {
      try
      {
        var results = await _repo.GetPalestrantesAsyncById(PalestranteId, true);
        return Ok(results);
      }
      catch (System.Exception ex)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco de Dados Falhou {ex.Message}");
      }
    }

    [HttpGet("getByName/{nome}")]
    public async Task<IActionResult> Get(string nome)
    {
      try
      {
        var results = await _repo.GetAllPalestrantesAsyncByName(nome, true);
        return Ok(results);
      }
      catch (System.Exception ex)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco Dados Falhou {ex.Message}");
      }
    }

    [HttpPost]
    public async Task<IActionResult> Post(Palestrante model)
    {
      try
      {
        _repo.Add(model);
        if (await _repo.SaveChangesAsync())
        {
          return Created($"/api/palestrante/{model.Id}", model);
        }
      }
      catch (System.Exception ex)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco de Dados Falhou {ex.Message}");
      }
      return BadRequest();
    }

    [HttpPut]
    public async Task<IActionResult> Put(int PalestranteId, Palestrante model)
    {
      try
      {
        var evento = await _repo.GetPalestrantesAsyncById(PalestranteId, false);
        if (evento == null) return NotFound();
        _repo.Update(model);

        if (await _repo.SaveChangesAsync())
        {
          return Created($"/api/palestrante/{model.Id}", model);
        }
      }
      catch (System.Exception ex)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco de Dados Falhou {ex.Message}");
      }
      return BadRequest();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int PalestranteId)
    {
      try
      {
        var palestrante = await _repo.GetPalestrantesAsyncById(PalestranteId, false);
        if (palestrante == null) return NotFound();
        _repo.Delete(palestrante);
      }
      catch (System.Exception ex)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco de Dados Falhou {ex.Message}");
      }
      return BadRequest();
    }
  }
}