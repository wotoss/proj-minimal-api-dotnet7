using System.Net;
using System.Text;
using System.Text.Json;
using Api.Test.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using proj_minimal_api_dotnet7.DTOs;
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
         var cliente = new ClienteDTO()
         {
          //Id = 1, não preciso enviar o id estamos no DTO
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

         //no meu retorno precisa ter o id
         var resultDoResponse = await response.Content.ReadAsStringAsync();
         //meu retorno aqui não mais uma lista e sim um cliente
         var clienteResponse = JsonSerializer.Deserialize<Cliente>(resultDoResponse, new JsonSerializerOptions
         {
          PropertyNameCaseInsensitive = true
         });
         Assert.IsNotNull(clienteResponse);
         Assert.IsNotNull(clienteResponse.Id);
         
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
    public async Task PutClientesPassandoPeloTeste()
    {
      /*
      * 1º envio o objeto que será serializado
      * 2º lembrando que nesta instância eu estou passando o ClienteDTO
      */
      var cliente = new ClienteDTO()
      {
        //Id = 1, não preciso enviar o id estamos no DTO
        Nome = "woto",
        Email = "wotoss10@gmail.com",
        Telefone = "(11) 94704-7361"

      };
         
         //preciso transformar JsonSerializer.Serialize(cliente) transformar o (objeto) para (string) com serialize
         var content = new StringContent(JsonSerializer.Serialize(cliente), Encoding.UTF8, "application/json");

         //veja que para fazer esta requisição eu passo a rota e objeto content
         //desta forma eu consigo fazer um post de uma string de um objeto parseado.
         //lembrando que a rota de put eu preciso passar id
         var response = await Setup.client.PutAsync($"/clientes/{1}", content);

         //neste momento do put o meu statusCode deve ser um ok
         Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

         //testar o tipo de retorno que vem desta api
         //seria uma api que vem 1º application/json 2º charset=utf-8
         //eu consigo testar o ContentType que vem da resposta desta api
         Assert.AreEqual("application/json; charset=utf-8", 
         response.Content.Headers.ContentType?.ToString());

         //no meu retorno precisa ter o id
         var resultDoResponse = await response.Content.ReadAsStringAsync();
         //meu retorno aqui não mais uma lista e sim um cliente
         var clienteResponse = JsonSerializer.Deserialize<Cliente>(resultDoResponse, new JsonSerializerOptions
         {
          PropertyNameCaseInsensitive = true
         });
         Assert.IsNotNull(clienteResponse);
         Assert.IsNotNull(clienteResponse.Id);         

    }

    [TestMethod]
    public async Task PutClientesSemNome()
    {
      //estou passando o objeto DTO sem a propriedade nome 
      //então eu espero um erro BadRequest
      var clienteSemNome = new ClienteDTO()
      {
        Email = "wotoss10@gmail.com",
        Telefone = "(11) 94704-7361"
      };
      //preciso transformar JsonSerializer.Serialize(cliente) transformar o (objeto) para (string) com serialize
      var content = new StringContent(JsonSerializer.Serialize(clienteSemNome), Encoding.UTF8, "application/json");
      //passando a rota put com paramentro id clientes/{1}
      var rotaPutComParametro = await Setup.client.PutAsync($"/clientes/{1}", content);
      
      Assert.AreEqual(HttpStatusCode.BadRequest, rotaPutComParametro.StatusCode);
      Assert.AreEqual("application/json; charset=utf-8", rotaPutComParametro.Content.Headers.ContentType?.ToString());
      
      var result = await rotaPutComParametro.Content.ReadAsStringAsync();
      Assert.AreEqual("""{"codigo":12345,"mensagem":"É preciso enviar o nome ele é obrigatório"}""", result);
    }

    [TestMethod]
    public async Task DeleteClientes()
    {
      var response = await Setup.client.DeleteAsync($"/clientes/{1}");
      Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
      //não preciso testar o retorno porque se NoContent ele não tem retorno.
    }

    //vamos passar um id invalido lá em clientes definimos o 6 como invalido
    [TestMethod]
    public async Task DeleteClientesIdNaoExistente()
    {
      var passandoIdInvalido = await Setup.client.DeleteAsync($"/clientes/{4}");
      Assert.AreEqual(HttpStatusCode.NotFound, passandoIdInvalido.StatusCode);
    }
/*
    //iremos fazer o verbo patch lembrando que o patch atualiza partes especificas
    [TestMethod]
    public async Task PatchClientes()
    {
      var cliente = new ClienteNomeDTO()
      {
        Nome = "woto",
      };
      var content = new StringContent(JsonSerializer.Serialize(cliente), Encoding.UTF8, "application/json-patch+json");
      var response = await Setup.client.PatchAsync($"/clientes/{1}", content);

      Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
      Assert.AreEqual("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

     
    }
*/

    [TestMethod]
    public async Task GetPorIdClienteNaoEncontrado()
    {
      var response = await Setup.client.GetAsync($"/clientes/{4}");
      Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    [TestMethod]
    public async Task GetPorId()
    {
      var response = await Setup.client.GetAsync($"/clientes/{1}");
      Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }


    /*
    * fazendo estes testes de integração na api
    * eu não preciso utilizar insominia, postman, swagger.
    */
}