
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace proj_minimal_api_dotnet7.Models;


[Table("administradores")]
public record Administrador
{
  public Administrador()
  {
     this.DataCriacao = DateTime.Now;
  }

  //este Identity é o autoIncremente
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public int Id { get; set; }
  
  [Required(ErrorMessage = "Nome é obrigatório")]
  [MaxLength(100)]
  public string Nome { get; set; } = default!;
  
  [Required(ErrorMessage = "Email é obrigatório")]
  [MaxLength(200)]
  public string Email { get; set; } = default!;

  
  [Required(ErrorMessage = "Senha é obrigatória")]
  [MaxLength(100)]
  public string Senha { get; set; } = default!;

  [Required(ErrorMessage = "Permissão é obrigatória")]
  [MaxLength(100)]
  public string Permissao { get; set; } = default!;
  public DateTime DataCriacao { get; set; } 

}