

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using proj_minimal_api_dotnet7.DTOs;
using proj_minimal_api_dotnet7.Infraestrutura.Interfaces;
using proj_minimal_api_dotnet7.Models;
using proj_minimal_api_dotnet7.ModelView;

namespace proj_minimal_api_dotnet7.Routes;

//estou fazendo como internal, porque não preciso compartilhar
// com outras partes da aplicação => como a parte de (api.test)
// quando eu coloco (internal) o test.api não enxerga
internal struct ClientesRoute
//como é uma struct eu não tenho construtor
{
    internal static void MapRoutes(IEndpointRouteBuilder app)
    {
       /*
    * Importante: este  (ClientesServico clientesServico) eu não estou
    recebendo como parametro. Eu estou injetando ele direto na minha rota

    * preciso colocar [FromServices] para o C# utilizar ou resolver a denpendencia
    Lembrando que passo no meu scoped => services.AddScoped<IBancoDeDadosServico<Cliente>, ClientesServico>();
      desta forma eu não estou trabalhando direto com a implementação ClientesServico
      e sim com o contrato => IBancoDeDadosServico<Cliente>
    */
    app.MapGet("/clientes", [Authorize] [Authorize(Roles = "editor, administrador")] async ([FromServices] IBancoDeDadosServico<Cliente> clientesServico) =>
    {
      //criar uma (objeto dynamic ou new) para criar uma lista vazia
      var clientes = await clientesServico.Todos();
      return Results.Ok(clientes);

    })
   .Produces<List<Cliente>>(StatusCodes.Status200OK)
   .WithName("GetClientes")
   .WithTags("Clientes");


    //vou decorar via anottion anotação (FromBody) para facilitar para minha aplicação
    //estou dizendo que vou receber esta informação
    //não é via [Post, Get, Form] é via [body] ou seja no corpo da mensagem
    app.MapPost("/clientes", [Authorize] [Authorize(Roles = "editor, administrador")] async ([FromServices] IBancoDeDadosServico<Cliente> clientesServico, [FromBody] ClienteDTO clienteDTO) =>
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
       await clientesServico.Salvar(cliente);

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
    app.MapPut("/clientes/{id}", [Authorize] [Authorize(Roles = "editor, administrador")] async ([FromServices] IBancoDeDadosServico<Cliente> clientesServico, [FromRoute] int id, [FromBody] ClienteDTO clienteDTO) =>
     {
        if(string.IsNullOrEmpty(clienteDTO.Nome))
        {
          //se eu não achar o meu cliente na bd eu retorno esta msg de error
          return Results.BadRequest(new Error
            {
              Codigo = 12345,
              Mensagem = "O Nome é obrigatório"
            });
        }//12345

      //validação
      //caso não ache meu cliente na base de dados.
      var clienteDb = await clientesServico.BuscaPorId(id);
      if(clienteDb is null)
      {
        //se eu não achar o meu cliente na bd eu retorno esta msg de error
         return Results.NotFound(new Error
          {
             Codigo = 423,
             Mensagem = $"Cliente não encontrado com o id {id}"
          });
      }     

       await clientesServico.Update(clienteDb, clienteDTO);
       return Results.Ok(clienteDb);
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
      app.MapPatch("/clientes/{id}", [Authorize] [Authorize(Roles = "editor, administrador")] async ([FromServices] IBancoDeDadosServico<Cliente> clientesServico, [FromRoute] int id, [FromBody] ClienteNomeDTO clienteNomeDTO) =>
      {

      //caso não ache meu cliente na base de dados.
      var clienteDb = await clientesServico.BuscaPorId(id);
      if(clienteDb is null)
      {
        //se eu não achar o meu cliente na bd eu retorno esta msg de error
         return Results.NotFound(new Error
          {
             Codigo = 2345,
             Mensagem = $"Cliente não encontrado com o id {id}"
          });
      }
       
       clienteDb.Nome = clienteNomeDTO.Nome;

       await clientesServico.Salvar(clienteDb);
       return Results.Ok(clienteDb);
        
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
      app.MapDelete("/clientes/{id}", [Authorize] [Authorize(Roles = "administrador")] async ([FromServices] IBancoDeDadosServico<Cliente> clientesServico, [FromRoute] int id) => 
      {
      //caso não ache meu cliente na base de dados.
      var clienteDb = await clientesServico.BuscaPorId(id);
      if(clienteDb is null)
      {
        //se eu não achar o meu cliente na bd eu retorno esta msg de error
         return Results.NotFound(new Error
          {
             Codigo = 22345,
             Mensagem = $"Cliente não encontrado com o id {id}"
          });
      }

        await clientesServico.Excluir(clienteDb);
         
         return Results.NoContent();
      }) 

      //As possibilidades de retorno
      .Produces<Cliente>(StatusCodes.Status204NoContent)
      .Produces<Error>(StatusCodes.Status404NotFound)
      .WithName("BuscaClientes")
      .WithTags("Clientes");


      //fazer o get de um objeto só
      app.MapGet("/clientes/{id}", [Authorize] [Authorize(Roles = "editor, administrador")] async ([FromServices] IBancoDeDadosServico<Cliente> clientesServico, [FromRoute] int id) => 
      {
        var clienteDb = await clientesServico.BuscaPorId(id);
           if(clienteDb is null)
           {
             //se eu não achar o meu cliente na bd eu retorno esta msg de error
             return Results.NotFound(new Error
               {
                 Codigo = 21345,
                 Mensagem = $"Cliente não encontrado com o id {id}"
               });
           }
          
          return Results.Ok(clienteDb);
          //return Results.OK(clienteDb);
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
}