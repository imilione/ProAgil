using System.Collections.Generic;

namespace ProAgil.WebAPI.DTOs
{
  public class EventoDTO
  {
    public int Id { get; set; }
    public string Local { get; set; }
    public string DataEvento { get; set; }
    public string Tema { get; set; }
    public int QtdPessoas { get; set; }
    public string ImagemURL { get; set; }
    public string Telefone { get; set; }
    public string Email { get; set; }
    public List<LoteDTO> Lotes { get; set; }
    public List<RedeSocialDTO> RedeSociais { get; set; }
    public List<PalestranteDTO> Palestrantes { get; set; }
  }
}