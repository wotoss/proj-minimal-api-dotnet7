/*
estou colocando o namespace na estrutura do C# 10 para cima
a estrutura sem ter bloco.
*/
namespace proj_minimal_api_dotnet7.ModelView;
//namespace api2;

//poderia ser um class record ou struct 
//como esta será uma classe super simples não terá muita informação de dados
//posso utilizar o (struct)
public struct Error
{
  //maperar meus erros por codigos especificos
  //ainda passei como required, coloque como obrigatorio
  //se não passar o codigo eu não permito criar a instância do objeto 
   public required int Codigo {get; set;}
   public required string Mensagem {get; set;}
   
}