using System.Net;
using Api.Test.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using proj_minimal_api_dotnet7;


namespace api.test.Requests;

[TestClass]
public class HomeRequestTest
{
[ClassInitialize]
  public static void ClassInit(TestContext testContext)
  {
    Setup.ClassInit(testContext);
  }

[ClassCleanup]
  public static void ClassCleanup()
  {
    Setup.ClassCleanup();
  }


    [TestMethod]
    public async Task TestaSeAHomeDaAPIExiste()
    {
       

        
        //vou fazer uma requisição para minha home e o retorno é 200
        var response = await Setup.client.GetAsync("/");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        //testar o tipo de retorno que vem desta api
        //seria uma api que vem 1º application/json 2º charset=utf-8
        //eu consigo testar o ContentType que vem da resposta desta api
        Assert.AreEqual("application/json; charset=utf-8", 
        response.Content.Headers.ContentType?.ToString());

        //outro exemplo como eu faço para testar o retorno
        //o meu Content ou seja o meu conteudo vem alguma coisa e eu quero
        //testar o que esta vindo neste conteudo ou Content
        var result = await response.Content.ReadAsStringAsync();
        Assert.AreEqual("""{"mensagem":"Seja bem vindo a API"}""", result);

    }

    [TestMethod]
    public async Task TestandoCaminhoCorretoRecebendoParametro()
    {
      //se você vair retornnar created o certo é enviar um postAsync você esta criando algo
      //mas como forma de exemplo vamos enviar um como GetAsync
      var response = await Setup.client.GetAsync("/recebe-parametro?nome=Leandro");
      Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
      Assert.AreEqual("application/json; charset=utf-8", 
        response.Content.Headers.ContentType?.ToString());
      

      var result = await response.Content.ReadAsStringAsync();
      Assert.AreEqual("""{"parametroPassado":"Alterando parametro recebido Leandro;","mensagem":"Parabens foi passado parametro por querystring"}""", result);

    }

    [TestMethod]
    public async Task TestandoRecebendoParametroSemOParametro()
    {
      //se você vair retornnar created o certo é enviar um postAsync você esta criando algo
      //mas como forma de exemplo vamos enviar um como GetAsync
      var response = await Setup.client.GetAsync("/recebe-parametro");
      
      //neste caso não estou passando o parametro. O que ocasionará erro
      //por isto espero um retorno BadRequest
      Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
      
      //vamos testar a mensagem digamos que ela seja uma regra denegocio
      var result = await response.Content.ReadAsStringAsync();
      Assert.AreEqual("""{"mensagem":"Olá o nome é obrigatório, por favor escreva."}""", result);
    }
}