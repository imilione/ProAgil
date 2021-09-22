using System;

namespace ProAgil.Domain
{
  public class Lote
  {
    public int Id { get; set; }
    public string Nome { get; set; }
    public decimal Preco { get; set; }
    public DateTime? DataInicio { get; }
    public DateTime? DataFim { get; }
    public int Quantidade { get; set; }
    public int EventoId { get; set; }
    public Evento Evento { get; }
  }
}