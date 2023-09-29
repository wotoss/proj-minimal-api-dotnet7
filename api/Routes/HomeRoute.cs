

using Microsoft.AspNetCore.Authorization;

namespace proj_minimal_api_dotnet7.Routes;

//estou fazendo como internal, porque não preciso compartilhar
// com outras partes da aplicação => como a parte de (api.test)
// quando eu coloco (internal) o test.api não enxerga
internal struct HomeRoute
//como é uma struct eu não tenho construtor
{
  /*
  * 1º Pego a instância do app e champ MapGet "eu estou usando o verbo get do protocolo http"
    2º Quando eu passo que a minha rota é barra "/" eu direcionando para minha rota inicial 
    3º arrow function () => quando eu coloco o 
    4º new estou utilizando um objeto dynamyc
    exemplo:
    4º a= dynamic app2 = new {Mensagem = "Seja bem vindo"}
    Desta forma quando faço uso do new eu estou dando um retorno de um objeto dinâmico 
    que tem uma propriedade chamada mensagem.
  */

  /*
  * 2º exemplo
  * vou criar uma rota get que vai receber um parametro, este parametro sera do tipo string
    2º vou fazer o bloco da rota com parametro { }
  */

    internal static void MapRoutes(IEndpointRouteBuilder app)
    {
        //estou dizendo que esta rota é anonima [AllowAnonymous] ou seja não quero que seja autenticada
    app.MapGet("/", [AllowAnonymous] () => new {Mensagem = "Seja bem vindo a API"})
    //aqui estou modelando os possiveis retornos desta parte (home) de 
    //minha api.
    //englobada ou feito um grupo pela (tag-Teste)
    .Produces<dynamic>(StatusCodes.Status200OK)
    .WithName("Home")
    .WithTags("Testes");

    //estou dizendo que esta rota é anonima [AllowAnonymous] ou seja não quero que seja autenticada
    app.MapGet("/recebe-parametro", [AllowAnonymous] (string? nome) =>
    {
      //montando um retorno na api caso venha com nome vazio
      if (string.IsNullOrEmpty(nome))
      {
        //este retorno (BadRequest - 400) voltará pois não passou na validação.
        return Results.BadRequest(new
        {
          Mensagem = "Olá o nome é obrigatório, por favor escreva."
        });
      }

      //fazer um template string """ """ com dotnet 7 
      //no dotnet 6 não é possivel deste formato
      nome = $"""
    Alterando parametro recebido {nome};
    """;

      //nome = "Alterando parametro recebido" + nome;

      var objetoDeRetorno = new
      {
        parametroPassado = nome,
        Mensagem = "Parabens foi passado parametro por querystring"
      };
      //return objetoDeRetorno;
      return Results.Created($"/recebe-parametro/{objetoDeRetorno.parametroPassado}", objetoDeRetorno);
      //vou dizer quais são os meus possiveis tipos de retorno
      //nos meus produces
    })
    //estou modelando o meu retorno com (201 ou 400)
    .Produces<dynamic>(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status400BadRequest)
    //neste momento (WithName) eu consigo dar um nome para rota
    .WithName("TesteRebeParametro")
    //com esta (tag - Testes) eu englobo tudo dentro de um grupo
    //estou englobando todos os testes que eu preciso em uma tag
    //se eu precisar fazer um (find / busca) de uma tag especifica eu consigo fazer
    .WithTags("Testes");

    }

}