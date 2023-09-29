

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using proj_minimal_api_dotnet7.DTOs;
using proj_minimal_api_dotnet7.Infraestrutura.Interfaces;
using proj_minimal_api_dotnet7.Models;
using proj_minimal_api_dotnet7.Servicos;

namespace proj_minimal_api_dotnet7.Routes;

//estou fazendo como internal, porque não preciso compartilhar
// com outras partes da aplicação => como a parte de (api.test)
// quando eu coloco (internal) o test.api não enxerga
internal struct AdministradoresRoute
//como é uma struct eu não tenho construtor
{

    internal static void MapRoutes(IEndpointRouteBuilder app)
    {
    //estou dizendo que esta rota é anonima [AllowAnonymous] ou seja não quero que seja autenticada
    app.MapPost("/login", [AllowAnonymous] async ( 
      //estou injetando estes objetos serviço para uso
      [FromServices] ILogin<Administrador> administradoresServico,
      [FromBody] LoginDTO login
     ) =>
    {
      //montando um retorno na api caso venha com nome vazio
      if (string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Senha))
      {
        //este retorno (BadRequest - 400) voltará pois não passou na validação.
        return Results.BadRequest(new
        {
          Mensagem = "Email e Senha são obrigatório"
        });
      }

      var admEncontrado = await administradoresServico.LoginAsync(login.Email, login.Senha);
      //este if eu posso colocar em uma linha só, por se tratar de apenas um tratamento
      if (admEncontrado is null) return Results.Unauthorized();
           
      var adm = new AdministradorLogadoDTO
      {
        Email = admEncontrado.Email,
        Senha = admEncontrado.Senha,
        Permissao = admEncontrado.Permissao,
        //lembrando: que estou gerando o token para o administrador
        //que foi encontrado na busca.
        Token = TokenServico.Gerar(admEncontrado)
      };

      return Results.Ok(adm);
      
    })
    //estou modelando o meu retorno com (201 ou 400)
    .Produces<dynamic>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status401Unauthorized)
    .Produces(StatusCodes.Status400BadRequest)
    //neste momento (WithName) eu consigo dar um nome para rota
    .WithName("Login")
    //com esta (tag - Administradores) eu englobo tudo dentro de um grupo
    //estou englobando todos os testes que eu preciso em uma tag
    //se eu precisar fazer um (find / busca) de uma tag especifica eu consigo fazer
    .WithTags("Administradores");

   // }

    //APENAS COMO TESTE PARA GERAR UM ADMINISTRADOR
    //estou dizendo que esta rota é anonima [AllowAnonymous] ou seja não quero que seja autenticada
    app.MapPost("/gerar-administrador-e-editor", [AllowAnonymous] async ( 
      //estou injetando estes objetos serviço para uso
      [FromServices] ILogin<Administrador> administradoresServico
      
     ) =>
    {
      

      var admEncontrado = await administradoresServico.LoginAsync("wotoss10@gmail.com", "123456");
      var admEditor = await administradoresServico.LoginAsync("suporte@wotoss10gmail.com", "123456");
      
      //vou criar um administrador
      if(admEncontrado is null)
      {
         await administradoresServico.Salvar(new Administrador
         {
           Nome = "woto",
           Email = "wotoss10@gmail.com",
           Senha = "123456",
           Permissao = "administrador"
         });
      }

      //se eu não achar o editor
      if(admEditor is null)
      {
         await administradoresServico.Salvar(new Administrador
         {
           Nome = "suporte",
           Email = "suporte@wotoss10gmail.com",
           Senha = "123456",
           Permissao = "editor"
         });
      }

      return Results.Ok(new 
      {
          Administrador = admEncontrado,
          Editor = admEditor
      });
      
    })
    //estou modelando o meu retorno com (201 ou 400)
    .Produces<dynamic>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status401Unauthorized)
    .Produces(StatusCodes.Status400BadRequest)
    //neste momento (WithName) eu consigo dar um nome para rota
    .WithName("GeraADM")
    //com esta (tag - Administradores) eu englobo tudo dentro de um grupo
    //estou englobando todos os testes que eu preciso em uma tag
    //se eu precisar fazer um (find / busca) de uma tag especifica eu consigo fazer
    .WithTags("Administradores");



    ///OUTRO TESTE-
    app.MapPost("/administradores", [AllowAnonymous] async ( 
      //estou injetando estes objetos serviço para uso
      [FromServices] ILogin<Administrador> administradoresServico
      
     ) =>
    {
      return Results.Ok(await administradoresServico.Todos());
    })
    //estou modelando o meu retorno com (201 ou 400)
    .Produces<dynamic>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status401Unauthorized)
    .Produces(StatusCodes.Status400BadRequest)
    //neste momento (WithName) eu consigo dar um nome para rota
    .WithName("ListaAdministradores")
    //com esta (tag - Administradores) eu englobo tudo dentro de um grupo
    //estou englobando todos os testes que eu preciso em uma tag
    //se eu precisar fazer um (find / busca) de uma tag especifica eu consigo fazer
    .WithTags("Administradores");

    }

    }
   

