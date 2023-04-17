

namespace proj_minimal_api_dotnet7.Infraestrutura.Interfaces;

/*
* Lembrando que a interface é um contrato, todos que
* herdarem seguiram o contrato
*/
public interface IBancoDeDadosServico<T>
{
  /*
  * Eu não quiz criar um generico por enquanto 
  * Então vou na estratégia de utilizar (objeto dinâmico)
  */

  Task Salvar (T objeto);

  Task Excluir (T objeto);
  Task ExcluirPorId(int id);
  Task<T> BuscaPorId(int id);
  Task<List<T>> Todos();

  /*
  * Aqui no meu contrato eu defino se eu quero trabalhar com 
  * programação paralela para ter uma performace melhor 
  * ao inves de eu ter um retorno de 
  * (void) eu tenho um retorno de um (task)
   sem programação paralela
   void Salvar (T objeto);
   com programação paralela
   task Salvar (T objeto);


  */
}