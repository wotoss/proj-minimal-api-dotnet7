using System.ComponentModel.DataAnnotations;

namespace proj_minimal_api_dotnet7.DTOs;
//namespace api2;

/*
poderia usar um (struct) para alocar na (memoria stack)
mas como vou precisar utilizar o (reflection) então é melhor colocar esta 
classe como (record). 
porque com (struct) eu não consigo utilizar o (reflection no meu objeto)

como vamos transferir dados com o DTO possivelmente usaremos o reflection
por este motivo é importante que seja uma (classe ou record)
*/
public record AdministradorLogadoDTO
{
 /*
 * por ser um objeto DTO (Objeto de Trânsferencia de Dados)
 * eu não vou passar o Id,  por ser um modelo. Como estou enivando
 * no (verbo post) ele criará o Id automaticamente. 
 */ 
 [Required]
  public required string Email {get; set;}
  
  //estou passando todas as propriedades como
  //(required) ou seja não conseguirei (criar o objeto) 
  //se tiver faltando alguma informação
 

  //esta (permissão) é o (perfil) do usuário
  public required string Permissao { get; set; } 

  //este será o token do (jwt)
  public required string Token { get; set; } 

}
/*
 Por isto o required
 Eu preciso que no DTO tenha todas estas informações
 Se não, não deixarei nem criar.
*/