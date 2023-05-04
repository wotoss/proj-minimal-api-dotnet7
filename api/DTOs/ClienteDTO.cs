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
public class ClienteDTO
{
 /*
 * por ser um objeto DTO (Objeto de Trânsferencia de Dados)
 * eu não vou passar o Id,  por ser um modelo. Como estou enivando
 * no (verbo post) ele criará o Id automaticamente. 
 */ 
 [Required]
  public string? Nome {get; set;} 

 [Required]
  public string? Telefone {get; set;} 

  public string? Email {get; set;}
}