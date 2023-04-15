using Microsoft.AspNetCore.Mvc;
using proj_minimal_api_dotnet7.DTOs;
using proj_minimal_api_dotnet7.Models;
using proj_minimal_api_dotnet7.ModelView;
using Microsoft.AspNetCore.Http;

namespace proj_minimal_api_dotnet7;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration? Configuration { get;set; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
       
        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            //endpoints.MapControllers();
            MapRoutes(endpoints);
            MapRoutesClientes(endpoints);
        });
    }

     #region Rotas utilizando Minimal API 
  public void MapRoutes(IEndpointRouteBuilder app)
  {
    app.MapGet("/", () => new {Mensagem = "Seja bem vindo a API"})
    //aqui estou modelando os possiveis retornos desta parte (home) de 
    //minha api.
    //englobada ou feito um grupo pela (tag-Teste)
    .Produces<dynamic>(StatusCodes.Status200OK)
    .WithName("Home")
    .WithTags("Testes");
    app.MapGet("/recebe-parametro", (string? nome) =>
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

  #endregion

  #region post

  //montagem de implementação verbo post
  public void MapRoutesClientes(IEndpointRouteBuilder app)
  {

    app.MapGet("/clientes", () =>
    {
      //criar uma (objeto dynamic ou new) para criar uma lista vazia
      var clientes = new List<Cliente>();

      /*
      * vou fazer um retorno mocado de clientes
      * 1º eu crio a instância Cliente 
      * 2º depois eu passo valores conforme a propriedade
      * 3º Não posso esquecer de dar o Add
      */
      clientes.Add(new Cliente()
      {
         Id = 1,
         Nome = "woto",
         Email = "wotoss10@gmail.com",
         Telefone = "(11) 94704-7361"
      });


      //var clientes = ClientesSevice.Todos();
      return Results.Ok(clientes);

    })
   .Produces<List<Cliente>>(StatusCodes.Status200OK)
   .WithName("GetClientes")
   .WithTags("Clientes");


    //vou decorar via anottion anotação (FromBody) para facilitar para minha aplicação
    //estou dizendo que vou receber esta informação
    //não é via [Post, Get, Form] é via [body] ou seja no corpo da mensagem
    app.MapPost("/clientes", ([FromBody] ClienteDTO clienteDTO) =>
     {
       /*
       estamos recebendo o cliente da view, html, formulario por parametro (clienteDTO)
       e vamos preencher a (instância ou classe) da (variavel cliente)
       */
       var cliente = new Cliente
       {
         /*
           não vou passar o id pois estou utilizando o método (post)
           quando eu utilizar o serviço ou serviçe para salvar 
           ele vai salvar a minha classe cliente e nesta classe ele terá 
           o Id dinamico.
           Por isto na DTO ou na ViewModel não preciso usar o id ou se fosse uma senha
          */
         Nome = clienteDTO.Nome,
         Telefone = clienteDTO.Telefone,
         Email = clienteDTO.Email,
       };

       //usando service 
       //ClienteService.Salvar(cliente);

       //Quando eu faço um post eu estou criando (Created) uma informação 
       //no return create passo a minha rota com Id do cliente especifico
       //em logo passo o objeto cliente que esta vindo por paramentro.
       return Results.Created($"/clientes/{cliente.Id}", cliente);
     })
    .Produces<Cliente>(StatusCodes.Status201Created)
    .Produces<Error>(StatusCodes.Status400BadRequest)
    .WithName("PostClientes")
    .WithTags("Clientes");




    //#region Put update


    /*
    * poderia usar o MapPatch caso eu fosse alterar apenas (parte da informação ou uma propriedade)
    * por exemplo = se eu quisesse alterar só nome, ou o email
    * do meu [ objeto, classe, record, struct ]
    * seguindo:
    * pela minha rota put eu espero o id, por isto faço est definição  [FromRoute] int id,
    * descrevendo a url: o que vou receber via http
    * 1º esta minha representação /clientes/{id}, tem que ser igual ao ([FromRoute] int id
    * 2º vou receber tambem o meu objeto de transferencia DTO pelo corpo da minha aplicação [FromBody] ClienteDTO clienteDTO)
    */
    app.MapPut("/clientes/{id}", ([FromRoute] int id, [FromBody] ClienteDTO clienteDTO) =>
     {

       if (string.IsNullOrEmpty(clienteDTO.Nome))
       {
         return Results.BadRequest(new Error
         {
           Codigo = 12345,
           Mensagem = "É preciso enviar o nome ele é obrigatório"
         });
       }


       /*
       var cliente = ClienteService.BuscaPorId(id);
       if(cliente == null)
       {
         return Results.NotFound(
           new Error {
             Codigo = 1234, 
             Mensagem = "Você passou um cliente inesistente"
           }
         )
       }
       cliente.Nome =clienteDTO.Nome;
       cliente.Telefone = clienteDTO.Telefone;
       cliente.Email = clienteDTO.Email;
       ClienteService.Update(cliente);
       */

       var cliente = new Cliente();
       /*
        * veja que o retorno eu mudo para Ok quero dizer foi atualizado
       */
       return Results.Ok(cliente);
     })
      //Pode me retornar um 200Ok se deu tudo certo
      .Produces<Cliente>(StatusCodes.Status200OK)
      //Pode me retornar um 404 caso ele não ache o id
      .Produces<Error>(StatusCodes.Status404NotFound)
      //Pode me retornar um 400 BadRequest caso ele não passe na validação.
      .Produces<Error>(StatusCodes.Status400BadRequest)
      //aqui eu tenho um tipo de rota chamado (PutClientes)
      .WithName("PutClientes")
      //aqui eu tenho a tag onde eu monto os meus grupos (Clientes)
      .WithTags("Clientes");


      //montando o método patch
      //estou recebendo o (id e o nomeclienteDTO)
      app.MapPatch("/clientes/{id}", ([FromRoute] int id, [FromBody] ClienteNomeDTO clienteNomeDTO) =>
      {

     if(string.IsNullOrEmpty(clienteNomeDTO.Nome))
       {
            // return Results.BadRequest(new Error
            // {
            //   Codigo = 123,
            //   Mensagem = "O Nome é obrigatório"
            // });
        }

        /*
        TODO fazer update cliente
        */
        

        var cliente = new Cliente();
        //return Results.OK(cliente);
       })
        //Pode me retornar um 200Ok se deu tudo certo
      .Produces<Cliente>(StatusCodes.Status200OK)
      //Pode me retornar um 404 caso ele não ache o id
      .Produces<Error>(StatusCodes.Status404NotFound)
      //Pode me retornar um 400 BadRequest caso ele não passe na validação.
      .Produces<Error>(StatusCodes.Status400BadRequest)
      //aqui eu tenho um tipo de rota chamado (PutClientes)
      .WithName("PatchClientes")
      //aqui eu tenho a tag onde eu monto os meus grupos (Clientes)
      .WithTags("Clientes");

      


      //montanto o método Delete
      //estou recebendo o paramentro lá do meu teste
      app.MapDelete("/clientes/{id}", ([FromRoute] int id) => 
      {
         if(id == 4)
         {
           return Results.NotFound(new Error
           {
            //passo um codido especifico 12
             Codigo = 12,
             Mensagem = "Cliente não encontrado"
           });
         }
         //TODO fazer a implementação para excluir da base de dados.
         return Results.NoContent();
      }) 

      //As possibilidades de retorno
      .Produces<Cliente>(StatusCodes.Status204NoContent)
      .Produces<Error>(StatusCodes.Status404NotFound)
      .WithName("BuscaClientes")
      .WithTags("Clientes");


      //fazer o get de um objeto só
      app.MapGet("/clientes/{id}", ([FromRoute] int id) => 
      {
         if(id == 4)
         {
           return Results.NotFound(new Error
           {
            //passo um codido especifico 12
             Codigo = 12,
             Mensagem = "Cliente não encontrado"
           });
         }
         //TODO fazer a implementação para excluir da base de dados.
         return Results.Ok(new Cliente()
         {
            Id = 1,
            Nome = "Yam",
            Telefone = "(11) 94704-7361",
            Email = "wotoss10gmail.com"
         });
      }) 

      //As possibilidades de retorno
      .Produces<Cliente>(StatusCodes.Status204NoContent)
      .Produces<Error>(StatusCodes.Status404NotFound)
      .WithName("GetClientesPorId")
      .WithTags("Clientes");




    //TODO FAZER A ROTA PATH "com ViewModel"
    //TODO FAZER A ROTA DELETE 
    //TODO FAZER A ROTA GET CLIENTE POR ID "retorna só o cliente"

    //TODO fazer testes request 
    //TODO fazer testes com postman ou insomminia
    //TODO fazer testes via curl

    /*
    * fazendo estes testes de integração na api
    * eu não preciso utilizar insominia, postman, swagger.

    * => a minha classe startup neste contexto minimal api faz o papel de controlador.
    */

  }
  //#endregion
  #endregion


}
