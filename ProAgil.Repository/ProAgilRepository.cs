using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProAgil.Domain;

namespace ProAgil.Repository
{
  public class ProAgilRepository : IProAgilRepository
  {
    public readonly ProAgilContext _context;
    public ProAgilRepository(ProAgilContext context)
    {
      _context = context;
      _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }
    public void Add<T>(T entity) where T : class
    {
      _context.Add(entity);
    }

    public void Delete<T>(T entity) where T : class
    {
      _context.Remove(entity);
    }
    public void Update<T>(T entity) where T : class
    {
      _context.Update(entity);
    }

    public async Task<bool> SaveChangesAsync()
    {
      return (await _context.SaveChangesAsync()) > 0;
    }

    public async Task<Evento[]> GetAllEventoAsync(bool includePalestrantes = false) //false means its not required
    {
      IQueryable<Evento> query = _context.Eventos
        .Include(c => c.Lotes)
        .Include(c => c.RedeSociais);

      if (includePalestrantes)
      {
        query = query
          .Include(pe => pe.PalestranteEventos)
          .ThenInclude(p => p.Palestrante);
      }

      query = query.OrderByDescending(c => c.DataEvento);

      return await query.ToArrayAsync();
    }

    ////////////////////////////////////////////////////////////////////////////////////
    //EVENTO
    public async Task<Evento> GetEventoAsyncById(int EventoId, bool includePalestrantes)
    {
      IQueryable<Evento> query = _context.Eventos
       .Include(c => c.Lotes)
       .Include(c => c.RedeSociais);

      if (includePalestrantes)
      {
        query = query
          .Include(pe => pe.PalestranteEventos)
          .ThenInclude(p => p.Palestrante);
      }

      query = query.OrderByDescending(c => c.DataEvento)
        .Where(c => c.Id == EventoId);

      return await query.FirstOrDefaultAsync();
    }
    public async Task<Evento[]> GetAllEventoAsyncByTema(string tema, bool includePalestrantes)
    {
      IQueryable<Evento> query = _context.Eventos
        .Include(c => c.Lotes)
        .Include(c => c.RedeSociais);

      if (includePalestrantes)
      {
        query = query
          .Include(pe => pe.PalestranteEventos)
          .ThenInclude(p => p.Palestrante);
      }

      query = query.OrderByDescending(c => c.DataEvento)
        .Where(c => c.Tema.ToLower().Contains(tema.ToLower()));

      return await query.ToArrayAsync();
    }
    ////////////////////////////////////////////////////////////////////////////////////

    ////////////////////////////////////////////////////////////////////////////////////
    //PALESTRANTE
    public async Task<Palestrante[]> GetAllPalestrantesAsyncByName(string name, bool includeEventos)
    {
      IQueryable<Palestrante> query = _context.Palestrantes
       .Include(c => c.RedeSociais);

      if (includeEventos)
      {
        query = query
          .Include(pe => pe.PalestranteEventos)
          .ThenInclude(e => e.Evento);
      }

      query = query.OrderBy(c => c.Nome)
        .Where(p => p.Nome.ToLower().Contains(name.ToLower()));

      return await query.ToArrayAsync();
    }
    public async Task<Palestrante> GetPalestrantesAsyncById(int PalestranteId, bool includeEventos = false)
    {
      IQueryable<Palestrante> query = _context.Palestrantes
       .Include(c => c.RedeSociais);

      if (includeEventos)
      {
        query = query
          .Include(pe => pe.PalestranteEventos)
          .ThenInclude(e => e.Evento);
      }

      query = query.OrderBy(c => c.Nome)
        .Where(p => p.Id == PalestranteId);

      return await query.FirstOrDefaultAsync();
    }
    ////////////////////////////////////////////////////////////////////////////////////
  }
}