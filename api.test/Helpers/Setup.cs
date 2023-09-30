using Api.Test.Mock;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using proj_minimal_api_dotnet7;
using proj_minimal_api_dotnet7.Infraestrutura.Database;
using proj_minimal_api_dotnet7.Infraestrutura.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using proj_minimal_api_dotnet7.Models;

namespace Api.Test.Helpers;

public class Setup
{

    /*
  posso para com _ this ou nome da classe HomeRequestTest
  mas neste ficaria melhor com _ 
  */
    public const string PORT = "5227";
    //public const string HOST =  "http://localhost";

    //fazendo correção do alerta em nullable (= default!)
    public static TestContext testContext = default!;
    //fazendo correção do alerta em nullable (= default!)
    public static WebApplicationFactory<Startup> http = default!;
    
    //quando for paramentros ou propriedade privadas usamos _
    public static HttpClient client = default!;
    
    /*
      1º estou fazendo um método genérico que recebe um parametro que seria 
      (string ou comando sql)  
       2º execultar comandos sql de forma generica
     em toda a aplicação (api.teste) caso precise
    */
    public static async Task ExecutaComandoSqlAsync(string sql)
      {
        await new DbContexto().Database.ExecuteSqlRawAsync(sql);
      } 

    /*
    * este termos como retorno um int 
    * a nossa string que esta vindo por paramentro ou noss comando
    * é um count => 
    */
     public static async Task<int> ExecutaEntityCountAsync(int id, string nome)
      {
        return await new DbContexto().Clientes.Where(c => c.Id == id && c.Nome == nome).CountAsync();
      }


    public static async Task FakeClienteAsync()
    {
      await new DbContexto().Database.ExecuteSqlRawAsync("""
      insert clientes(Nome, Telefone, Email, DataCriacao)
      values('Livia', '(11) 94704-7361', 'liviass@gmail.com', '2023-04-17 06:09:00')
      """);
    } 

     //[ClassInitialize]
    public static void ClassInit(TestContext testContext)
    {
      //como este método é statico eu tenho que passar com nome da minha classe
      //HomeRequestTest
      Setup.testContext = testContext;
      Setup.http = new WebApplicationFactory<Startup>();

      //deixo aqui esta configuração da porta para que não precise fazer 
      //em cada teste. Já otimizamos esta parte
      Setup.http = Setup.http.WithWebHostBuilder(builder =>
       {
         builder.UseSetting("https_port", Setup.PORT).UseEnvironment("Testing");

         builder.ConfigureServices(services =>
         {
          /*
          1º como esta classe Setup inicia o bot dos meus teste 
          eu estou services.AddScoped<IBancoDeDadosServico<Cliente>, ClientesServicoMoc>();
          para que ele resolva a implemetação de minha classe ClientesServicoMoc diante do contrato

          2º ao invês de eu fazer a conexão com a base de dados 3 utilizar o entity-framework,
          para ir no banco e fazer as requisições. Eu vou fazer um (MOC) desta classe para que ela 
          possar responder os dados em memoria.

          3º desta forma eu testo o fluxo do http. Eu não vou testar se esta inserindo na base de dados
          realmente ou não. Para isto seria um (teste de unidade) para testar os serviços se ele grava ou não
          
          Já os testes de (request) que são os testes de integração o objetivo deste teste é testar
          o fluxo do http se ele retorna 200 - OK se ele retorna 201 -Created - informação errada BadRequest
          formato que retorna o formato que tem que enviar para api.

          Lembrando base de dados é o teste de serviço de unidade. Já teste http são testes de integração

          4º Estou substituindo o services.AddScoped<IBancoDeDadosServico<Cliente>, ClientesServico>();
          Statup.
          Por esta do Setup services.AddScoped<IBancoDeDadosServico<Cliente>, ClientesServicoMoc>();
          Pois esta do Setup é a inicialização dos meus teste.
          E a da Startup é a inicialização do meu aplicativo.
          */
            services.AddScoped<IBancoDeDadosServico<Cliente>, ClientesServicoMock>();
            services.AddScoped<ILogin<Administrador>, AdministradoresServicoMock>();
         });
       });

       //como eu vou ter que passar em todos os testes 
       //então eu difino globalmente na minha inicialização
        Setup.client = Setup.http.CreateClient();

    }
    //usando a classe [ClassCleanup] para finalizar o estado do servidor.
    //fazendo um dispose do servidor.
    //[ClassCleanup]
    public static void ClassCleanup()
    {
      Setup.http.Dispose();
    }
}