
/*
estou colocando o namespace na estrutura do C# 10 para cima
a estrutura sem ter bloco.
*/
namespace proj_minimal_api_dotnet7.ErrorDTO;
//namespace api2;

//poderia ser um class record ou struct 
//como esta será uma classe super simples não terá muita informação de dados
//posso utilizar o (struct) = coloco na (memoria stack) 
public struct ErrorDTO 
{
  //maperar meus erros por codigos especificos
  //ainda passei como required, coloque como obrigatorio
  //se não passar o codigo eu não permito criar a instância do objeto 
   public required int Codigo {get; set;}
   public required string Mensagem {get; set;}
   
}