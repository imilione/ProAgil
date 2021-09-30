using System.ComponentModel.DataAnnotations;

namespace ProAgil.WebAPI.DTOs
{
  public class RedeSocialDTO
  {
    public int Id { get; set; }
    [Required(ErrorMessage = "O campo '{0}' é obrigatório.")]
    public string Nome { get; set; }
    [Required(ErrorMessage = "O campo '{0}' é obrigatório.")]
    public string URL { get; set; }
  }
}