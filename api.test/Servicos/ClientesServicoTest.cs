
using Api.Test.Helpers;
using proj_minimal_api_dotnet7.Infraestrutura.Database;
using proj_minimal_api_dotnet7.Models;
using proj_minimal_api_dotnet7.Servicos;

namespace api.test.Servico;

[TestClass]
public class ClientesServicoTest
{
  /*
    public static void ClassInit(TestContext testContext)
    seria a classe inicial
  */
[ClassInitialize]
  public static void ClassInit(TestContext testContext)
  {
    
  }

/*
  public static void ClassCleanup()
  classe de limpar ou fechar
*/
[ClassCleanup]
  public static void ClassCleanup()
  {
    
  }

/*
  public async Task TestaSeAHomeDaAPIExiste()
  aqui temos o método de teste unidade onde vou testar minha classe cliente
*/

    [TestMethod]
    public async Task TestaSalvarDadoNoBanco()
    {
       /*
       1º para fazer o teste de unidade no método Salvar,  eu preciso injetar meu dbContexto
       2º neste caso eu estou criando uma instância nova do meu DbContexto
       3º rodo o truncate no banco e ai eu posso criar um usúario fake.
       "ao dar o truncate eu garanto que a minha tabela esta vazia"
       */       

        await Setup.ExecutaComandoSqlAsync("truncate table clientes");

        var  clientesServico = new ClientesServico(new DbContexto());

        var cliente = new Cliente()
        {
        
          Nome = "Usuario Teste",
          Email = "usuario@teste.com",
          Telefone = "(11) 94704-7361",
        
        };
        /*
         este objeto pode ser declarado desta forma tambem
         cliente.Nome = "Usuario Teste";
         cliente.Email = "usuario@teste.com";
         cliente.Telefone = "(11) 94704-7361";
        */
      
        await clientesServico.Salvar(cliente);


        /*
          CONCEITO DO NOSSO TESTE
          1º Se eu dei um truncate na base de dados.
          "eu acabei de limpar esta base"
          2º Estou criando um cliente sem passar o Id
          "porque a minha base de dados esta LIMPA"
          "e o Id é um (auto incremente)", quando chegar na minha tabela 
          cliente por estar limpa ele vai me retornar um 1.

          3º Assert.AreEqual(1, cliente.Id);
          1 = é o que eu espero da base de dados
          cliente.Id é o que esta vindo da base de dados
        */
        

        Assert.AreEqual(1, cliente.Id);

    }

}

   