using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using proj_minimal_api_dotnet7;
using proj_minimal_api_dotnet7.Infraestrutura.Database;

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
    public static async Task ExecutaComandoSql(string sql)
      {
        await new DbContexto().Database.ExecuteSqlRawAsync(sql);
      } 

    /*
    * este termos como retorno um int 
    * a nossa string que esta vindo por paramentro ou noss comando
    * é um count => 
    */
     public static async Task<int> ExecutaEntityCount(int id, string nome)
      {
        return await new DbContexto().Clientes.Where(c => c.Id == id && c.Nome == nome).CountAsync();
      }


    public static async Task FakeCliente()
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