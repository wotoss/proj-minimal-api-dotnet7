using System.Net;
using System.Text;
using System.Text.Json;
using Api.Test.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

using proj_minimal_api_dotnet7;
using proj_minimal_api_dotnet7.Models;

namespace api.test.Requests;

[TestClass]
public class ClientesRequestTest
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
    public async Task GetDeClientesEmTeste()
    {      
        //vou fazer uma requisição para minha home e o retorno é 200
        var response = await Setup.client.GetAsync("/clientes");

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
        //com Deserilaze eu consigo fazer o meu teste sem fixar os valores
        var clientes = JsonSerializer.Deserialize<List<Cliente>>(result, new JsonSerializerOptions
        {
          PropertyNameCaseInsensitive = true
        });
        //teste feito pelo moc em clientes
        Assert.IsNotNull(clientes); //programação defensiva.
        Assert.IsTrue(clientes.Count > 0);
        Assert.IsNotNull(clientes[0].Id);
        Assert.IsNotNull(clientes[0].Nome);
        Assert.IsNotNull(clientes[0].Email);
        Assert.IsNotNull(clientes[0].DataCriacao);

    }

     [TestMethod]
     public async Task PostDeClientesEmTeste()
     {
         /*
         * 1º para fazer o post crio a instância do meu objeto cliente
         * 2º passo os valores
         * 3º envio ele através do payload
         */
         var cliente = new Cliente()
         {
          Id = 1,
          Nome = "woto",
          Email = "wotoss10@gmail.com",
          Telefone = "(11) 94704-7361"
         };
         //preciso transformar JsonSerializer.Serialize(cliente) transformar o (objeto) para (string) com serialize
         var content = new StringContent(JsonSerializer.Serialize(cliente), Encoding.UTF8, "application/json");

         //veja que para fazer esta requisição eu passo a rota e objeto content
         //desta forma eu consigo fazer um post de uma string de um objeto parseado.
         var response = await Setup.client.PostAsync("/clientes", content);
         
         //Ao eu mandar criar no servidor o retorno deve ser um staturs de created
         Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

         //testar o tipo de retorno que vem desta api
         //seria uma api que vem 1º application/json 2º charset=utf-8
         //eu consigo testar o ContentType que vem da resposta desta api
         Assert.AreEqual("application/json; charset=utf-8", 
         response.Content.Headers.ContentType?.ToString());

         
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
}