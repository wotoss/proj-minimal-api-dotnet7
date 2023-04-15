/*
estou colocando o namespace na estrutura do C# 10 para cima
a estrutura sem ter bloco.
*/
namespace proj_minimal_api_dotnet7.Models;
//namespace api2;
//poderia ser um class record ou struct 
public record Cliente 
{
   //vou passar no contrutor uma data. vou fazer isto para meu teste
   //como a data irá sempre mudar. tenho que ajustar no teste
   public Cliente ()
   {
      this.DataCriacao = DateTime.Now;
   }
   public int Id {get; set;}
   public string? Nome {get; set;}
   public string? Telefone {get; set;}
   public string? Email {get; set;}

   //apenas como teste 
   public DateTime DataCriacao {get; set;}

}