

namespace proj_minimal_api_dotnet7.Infraestrutura.Interfaces;

/*
* Lembrando que a interface é um contrato, todos que
* herdarem seguiram o contrato
*/

/*
Importante 
 Eu tenho uma interface de Login
 que tem tudo de banco de dados e tudo de login
 Isto é a herança na (interface ou contrato)
*/
public interface ILogin<T> : IBancoDeDadosServico<T>
{
  /*
  * Eu não quiz criar um generico por enquanto 
  * Então vou na estratégia de utilizar (objeto dinâmico)
  */
   Task<T?> LoginAsync(string email, string senha);
  
}