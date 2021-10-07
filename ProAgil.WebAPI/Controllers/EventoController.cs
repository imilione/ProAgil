using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProAgil.Domain;
using ProAgil.Repository;
using ProAgil.WebAPI.DTOs;

namespace ProAgil.WebAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class EventoController : ControllerBase
  {
    private readonly IProAgilRepository _repo;
    public IMapper _mapper { get; }
    public EventoController(IProAgilRepository repo, IMapper mapper)
    {
      this._mapper = mapper;
      _repo = repo;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
      try
      {
        var eventos = await _repo.GetAllEventoAsync(true);
        var results = _mapper.Map<EventoDTO[]>(eventos);
        return Ok(results);
      }
      catch (System.Exception ex)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco Dados Falhou {ex.Message}");
      }
    }

    [HttpPost("upload")]
    public async Task<IActionResult> upload()
    {
      try
      {
        var file = Request.Form.Files[0];
        var folderName = Path.Combine("Resources", "Images");
        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

        if (file.Length > 0)
        {
          var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
          var fullPath = Path.Combine(pathToSave, filename.Replace("\"", " ").Trim());

          using (var stream = new FileStream(fullPath, FileMode.Create))
          {
            file.CopyTo(stream);
          }
        }

        return Ok();
      }
      catch (System.Exception ex)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco Dados Falhou {ex.Message}");
      }
    }

    [HttpGet("{EventoId}")]
    public async Task<IActionResult> Get(int EventoId)
    {
      try
      {
        var evento = await _repo.GetEventoAsyncById(EventoId, true);
        var results = _mapper.Map<EventoDTO>(evento);

        return Ok(results);
      }
      catch (System.Exception ex)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco Dados Falhou {ex.Message}");
      }
    }


    [HttpGet("getByTema/{tema}")]
    public async Task<IActionResult> Get(string tema)
    {
      try
      {
        var eventos = await _repo.GetAllEventoAsyncByTema(tema, true);
        var results = _mapper.Map<EventoDTO[]>(eventos);
        return Ok(results);
      }
      catch (System.Exception ex)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco Dados Falhou {ex.Message}");
      }
    }

    [HttpPost]
    public async Task<IActionResult> Post(EventoDTO model)
    {
      try
      {
        var evento = _mapper.Map<Evento>(model);
        _repo.Add(evento);

        if (await _repo.SaveChangesAsync())
          return Created($"/api/evento/{model.Id}", _mapper.Map<EventoDTO>(evento));
      }
      catch (System.Exception ex)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco Dados Falhou {ex.Message}");
      }
      return BadRequest();
    }

    [HttpPut("{EventoId}")]
    public async Task<IActionResult> Put(int EventoId, EventoDTO model)
    {
      try
      {
        var evento = await _repo.GetEventoAsyncById(EventoId, false);
        if (evento == null) return NotFound();

        _mapper.Map(model, evento);

        _repo.Update(evento);

        if (await _repo.SaveChangesAsync())
          return Created($"/api/evento/{model.Id}", _mapper.Map<EventoDTO>(evento));
      }
      catch (System.Exception ex)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco Dados Falhou {ex.Message}");
      }
      return BadRequest();
    }

    [HttpDelete("{EventoId}")]
    public async Task<IActionResult> Delete(int EventoId)
    {
      try
      {
        var evento = await _repo.GetEventoAsyncById(EventoId, false);
        if (evento == null) return NotFound();
        _repo.Delete(evento);

        if (await _repo.SaveChangesAsync())
          return Ok();
      }
      catch (System.Exception ex)
      {
        return this.StatusCode(StatusCodes.Status500InternalServerError, $"Banco Dados Falhou {ex.Message}");
      }
      return BadRequest();
    }
  }
}