using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProAgil.WebAPI.DTOs
{
  public class EventoDTO
  {
    public int Id { get; set; }
    [Required(ErrorMessage = "O campo '{0}' é obrigatório.")]
    public string Local { get; set; }
    public string DataEvento { get; set; }
    public string Tema { get; set; }
    [Range(2, 120000, ErrorMessage = "Quantidade de pessoas deve ser entre 2 a 120.000.")]
    public int QtdPessoas { get; set; }
    public string ImagemURL { get; set; }
    public string Telefone { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    public List<LoteDTO> Lotes { get; set; }
    public List<RedeSocialDTO> RedeSociais { get; set; }
    public List<PalestranteDTO> Palestrantes { get; set; }
  }
}