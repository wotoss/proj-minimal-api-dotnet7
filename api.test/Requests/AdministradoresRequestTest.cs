using System.Net;
using System.Text;
using System.Text.Json;
using Api.Test.Helpers;
using proj_minimal_api_dotnet7.DTOs;
using proj_minimal_api_dotnet7.Models;

namespace api.test.Requests;

[TestClass]
public class AdministradoresRequestTest
{
[ClassInitialize]
  public static void ClassInit(TestContext testContext)
  {
    Setup.ClassInit(testContext);
    /*
     * 1º toda vez que a applicação iniciar em modo teste
     ela vai dar um (tuncate) na tabela (clientes)
     isto fará com que zere a tabela.

     Lembrando que estou fazendo na (força bruta de forma implicita)
      Database.ExecuteSqlRaw("truncate table clientes")

      2º Lá na pasta Helper do meu (api.test) eu tenho o arquivo ou Classe Setup.cs
      nesta classe eu tenho um método (genérico) que pode receber todas as execuções em sql
      Este método é Setup.ExecultaComandoSql(string sql). Lá no meu Setup.cs

      3º Apenas envio por parametro a String que eu quero que execulte.

    */
    //await Setup.ExecutaComandoSql("truncate table clientes");
  }

[ClassCleanup]
  public static void ClassCleanup()
  {
    Setup.ClassCleanup();
    /*
     vou fazer um truncate quando eu inicio ([ClassInitialize]) os teste
     e um outro truncate aqui [ClassCleanup] quando eu finalizo 
    */
    //await Setup.ExecutaComandoSql("truncate table clientes");
  }


    [TestMethod]
    public async Task GetDeClientesEmTeste()
    {  
        //Eu estou criando um usuário fake para teste
        //O método (FakeClientes()) esta lá no meu Setup
        //await Setup.FakeCliente();

        //vou fazer uma requisição para minha home e o retorno é 200
        var response = await Setup.client.GetAsync("/administradores");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        
    }

     [TestMethod]
     public async Task Login()
     {
         var loginDTO = new LoginDTO()
         {
          Email = "wotoss10@gmail.com",
          Senha = "123456"
         };
         //preciso transformar JsonSerializer.Serialize(cliente) transformar o (objeto) para (string) com serialize
         var content = new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8, "application/json");
         var response = await Setup.client.PostAsync("/login", content);
         
         //Ao eu mandar criar no servidor o retorno deve ser um staturs de created
         Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

         //testar o tipo de retorno que vem desta api
         //seria uma api que vem 1º application/json 2º charset=utf-8
         //eu consigo testar o ContentType que vem da resposta desta api
         Assert.AreEqual("application/json; charset=utf-8", 
         response.Content.Headers.ContentType?.ToString());

         //no meu retorno precisa ter o id
         var resultDoResponse = await response.Content.ReadAsStringAsync();
         //meu retorno aqui não mais uma lista e sim um cliente
         var admLogado = JsonSerializer.Deserialize<AdministradorLogadoDTO>(resultDoResponse, new JsonSerializerOptions
         {
          PropertyNameCaseInsensitive = true
         });
         Assert.IsNotNull(admLogado);
         Assert.IsNotNull(admLogado.Token); 
         
      }
}