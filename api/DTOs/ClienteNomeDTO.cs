namespace proj_minimal_api_dotnet7.DTOs;

public record ClienteNomeDTO
{
  //eu defino esta propriedade nome como required
  //sou obrigado a passar ela na criação.
  public required string Nome {get; set;}
}