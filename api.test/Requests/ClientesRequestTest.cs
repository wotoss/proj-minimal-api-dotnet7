using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Api.Test.Helpers;
using Api.Test.Mock;
using proj_minimal_api_dotnet7.DTOs;
using proj_minimal_api_dotnet7.Models;
using proj_minimal_api_dotnet7.Servicos;

namespace api.test.Requests;

[TestClass]
public class ClientesRequestTest
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

       await SetHeaderToken();
        //Eu estou criando um usuário fake para teste
        //O método (FakeClientes()) esta lá no meu Setup
        //await Setup.FakeCliente();

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

      await SetHeaderToken();
      /*
       Não preciso execultar truncate na base de dados, pois estou
      utilizando MOC e não base de dados
      */
      //await Setup.ExecutaComandoSql("truncate table clientes");
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
         //estou dizendo que meu Id é igual 1
         //pois sempre só terei um na base de dados.
         Assert.AreEqual(1, clienteResponse.Id); 
         
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
    /*
    TODO AJUSTAR DEPOIS ESTE TESTE PutClientesPassandoPeloTeste
    Vamos implementar depois !!!
    PutClientesPassandoPeloTeste()
    */
   // [TestMethod]
    //public async Task PutClientesPassandoPeloTeste()
   // {
      /*
       * comentei esta linhas pois não vou mais na minha base de dados
       * estarei usando a estratefia com (MOC)
       *
      */
     //await Setup.ExecutaComandoSql("truncate table clientes");
     //await Setup.FakeCliente(); 

     //var qtdInicial = await Setup.ExecutaEntityCount(1, "Livia");
     //Assert.AreEqual(1, qtdInicial);

      /*
      * 1º envio o objeto que será serializado
      * 2º lembrando que nesta instância eu estou passando o ClienteDTO
      */
      //var cliente = new ClienteDTO()
      //{
        //Id = 1, não preciso enviar o id estamos no DTO
       // Nome = "woto",
       // Email = "wotoss10@gmail.com",
        //Telefone = "(11) 94704-7361"

      //};
      
         
         //preciso transformar JsonSerializer.Serialize(cliente) transformar o (objeto) para (string) com serialize
    //      var content = new StringContent(JsonSerializer.Serialize(cliente), Encoding.UTF8, "application/json");
         
    //      //veja que para fazer esta requisição eu passo a rota e objeto content
    //      //desta forma eu consigo fazer um post de uma string de um objeto parseado.
    //      //lembrando que a rota de put eu preciso passar id
    //      var response = await Setup.client.PutAsync($"/clientes/{1}", content);

    //      //neste momento do put o meu statusCode deve ser um ok
    //      Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

    //      //testar o tipo de retorno que vem desta api
    //      //seria uma api que vem 1º application/json 2º charset=utf-8
    //      //eu consigo testar o ContentType que vem da resposta desta api
    //      Assert.AreEqual("application/json; charset=utf-8", 
    //      response.Content.Headers.ContentType?.ToString());

    //      //no meu retorno precisa ter o id
    //      var resultDoResponse = await response.Content.ReadAsStringAsync();
    //      //meu retorno aqui não mais uma lista e sim um cliente
    //      var clienteResponse = JsonSerializer.Deserialize<Cliente>(resultDoResponse, new JsonSerializerOptions
    //      {
    //       PropertyNameCaseInsensitive = true
    //      });
    //      Assert.IsNotNull(clienteResponse);
    //      //o id é 1 pois só tenho um cliente na bass de dados.
    //      Assert.AreEqual(1, clienteResponse.Id);   

    //      Assert.AreEqual("woto", clienteResponse.Nome); 

    // }

    [TestMethod]
    public async Task PutClientesSemNome()
    {
       await SetHeaderToken();

      /*
       retirei o Setup.FakeCliente(); pois não preciso mais de cliente fake
       estou usando a stratégia (MOC)
      */
      //await Setup.FakeCliente();
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
      //codigo":234312 É preciso enviar o nome ele é obrigatório
      var result = await rotaPutComParametro.Content.ReadAsStringAsync();
      Assert.AreEqual("""{"codigo":12345,"mensagem":"O Nome é obrigatório"}""", result);
    }

    [TestMethod]
    public async Task DeleteClientes()
    {
       await SetHeaderToken();

      /*
       retirei o Setup.FakeCliente(); pois não preciso mais de cliente fake
       estou usando a stratégia (MOC)
      */
      //await Setup.FakeCliente();
      var response = await Setup.client.DeleteAsync($"/clientes/{1}");
      Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
      //não preciso testar o retorno porque se NoContent ele não tem retorno.
    }

    //vamos passar um id invalido lá em clientes definimos o 6 como invalido
    [TestMethod]
    public async Task DeleteClientesIdNaoExistente()
    {
       await SetHeaderToken();
       
      //await Setup.ExecutaComandoSql("truncate table clientes");
      /*
        passando o usuario fake para minha base de dados 
        para ter o que buscar, por id 
      */
      /*
       retirei o Setup.FakeCliente(); pois não preciso mais de cliente fake
       estou usando a stratégia (MOC)
      */
      //await Setup.FakeCliente();
      var passandoIdInvalido = await Setup.client.DeleteAsync($"/clientes/{5}");
      Assert.AreEqual(HttpStatusCode.NotFound, passandoIdInvalido.StatusCode);
    }

    [TestMethod]
    public async Task GetPorIdClienteNaoEncontrado()
    {
      var response = await Setup.client.GetAsync($"/clientes/{4}");
      Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    [TestMethod]
    public async Task GetPorId()
    {
      /*
       Não preciso execultar truncate na base de dados, pois estou
      utilizando MOC e não base de dados
      */
      //await Setup.ExecutaComandoSql("truncate table clientes");
      /*
        sempre construindo meu usuario fake
        e limpando a base de dados na sequencia em minha 
        classe Staturp.cs =>(fazendo o truncate)
      */
      //await Setup.FakeCliente();
      var response = await Setup.client.GetAsync($"/clientes/{1}");
      Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }


    /*
    * fazendo estes testes de integração na api
    * eu não preciso utilizar insominia, postman, swagger.
    */


    private async Task SetHeaderToken()
    {
      if(Setup.client.DefaultRequestHeaders.Authorization is not null) return;
          
    //MAIS UM TESTE AUTENTICAÇÃO
       var loginDTO = new LoginDTO()
         {
          Email = "wotoss10@gmail.com",
          Senha = "123456"
         };
         //preciso transformar JsonSerializer.Serialize(cliente) transformar o (objeto) para (string) com serialize
         var content = new StringContent(JsonSerializer.Serialize(loginDTO), Encoding.UTF8, "application/json");
         var response = await Setup.client.PostAsync("/login", content);

         //no meu retorno precisa ter o id
         var resultDoResponse = await response.Content.ReadAsStringAsync();
         //meu retorno aqui não mais uma lista e sim um cliente
         var admLogado = JsonSerializer.Deserialize<AdministradorLogadoDTO>(resultDoResponse, new JsonSerializerOptions
         {
          PropertyNameCaseInsensitive = true
         });

         var token = admLogado?.Token;
    //FIM



    //no momento que eu crio o teste eu já faço a autenticação
    Setup.client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}