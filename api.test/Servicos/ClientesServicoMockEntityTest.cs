
using Api.Test.Helpers;
using Microsoft.EntityFrameworkCore;
using Moq;
using proj_minimal_api_dotnet7.Infraestrutura.Database;
using proj_minimal_api_dotnet7.Models;
using proj_minimal_api_dotnet7.Servicos;

namespace api.test.Servico;
/*
  * FAZENDO MOCK DO ENTITY
  AGORA VAMOS FAZER O TESTE UTILIZANDO MOCK NÃO BASE DE DADOS
  NESTE MOMENTO EU INSTALEI NUGET MOQ
  dotnet add package Moq --version 4.18.4
*/
[TestClass]
public class ClientesServicoMockEntityTest
{

    [TestMethod]
    public async Task TestaSalvar()
    {
      /*
        estou mocando a minha conexão com a base de dados
      */
       var mockContext = new Mock<DbContexto>();
       //estou mocando o DbSet<Cliente>
       var dbSetClientes = new Mock<DbSet<Cliente>>();

       mockContext.Setup(c => c.Clientes).Returns(dbSetClientes.Object);
       mockContext.Setup(c => c.SaveChanges()).Returns(1);
        
        /*
          quando eu faço esta criação não é mais o new Mock<DbContexto>(); original
          é o (DbContexto) mocado =>  new ClientesServico(mockContext.Object);
        */
       var clientesServico = new ClientesServico(mockContext.Object);
        
        /*
          2º PASSO
          utilizando a minha classe mocada
          *instanciando a classe e pasando os dados para salvar no banco
        */
        var cliente = new Cliente()
        {
        
          Nome = "Usuario Teste",
          Email = "usuario@teste.com",
          Telefone = "(11) 94704-7361",
        
        };

        await clientesServico.Salvar(cliente);
    }

    [TestMethod]
    public async Task TestaTodos()
    {
      /*
        quero que retorne uma lista com 3 clientes
        ou seja retorne uma lista
      */
      var lista = new List<Cliente>
      {
        new Cliente { Id = 1, Nome = "BBB" },
        new Cliente { Id = 2, Nome = "ZZZ" },
        new Cliente { Id = 3, Nome = "AAA" },
      };
      var data = lista.AsQueryable();

      var mockContext = new Mock<DbContexto>();
      var mockSet = new Mock<DbSet<Cliente>>();
    
      /*  
          aqui eu faço a configuração da minha lista
      */
      mockSet.As<IQueryable<Cliente>>().Setup(m => m.Provider).Returns(data.Provider);
      mockSet.As<IQueryable<Cliente>>().Setup(m => m.Expression).Returns(data.Expression);
      mockSet.As<IQueryable<Cliente>>().Setup(m => m.ElementType).Returns(data.ElementType);
      mockSet.As<IQueryable<Cliente>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator);  
      mockContext.Setup(c => c.Clientes).Returns(mockSet.Object);

      var clientesServico = new ClientesServico(mockContext.Object);

      var listaRetorno =  await clientesServico.Todos();

      Assert.AreEqual(3, listaRetorno.Count);     

    }

    /*
      Teste busca por id
    */
    [TestMethod]
    public async Task TestaBuscaPorId()
    {
      /*
        buscando por id
      */
      var lista = new List<Cliente>
      {
        new Cliente { Id = 1, Nome = "livia" },
        new Cliente { Id = 2, Nome = "vania" },
        new Cliente { Id = 3, Nome = "woto" },
      };
      var data = lista.AsQueryable();

      var mockContext = new Mock<DbContexto>();
      var mockSet = new Mock<DbSet<Cliente>>();
    
      /*  
          aqui eu faço a configuração da minha lista
      */
      mockSet.As<IQueryable<Cliente>>().Setup(m => m.Provider).Returns(data.Provider);
      mockSet.As<IQueryable<Cliente>>().Setup(m => m.Expression).Returns(data.Expression);
      mockSet.As<IQueryable<Cliente>>().Setup(m => m.ElementType).Returns(data.ElementType);
      mockSet.As<IQueryable<Cliente>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator);  
      mockContext.Setup(c => c.Clientes).Returns(mockSet.Object);

      var clientesServico = new ClientesServico(mockContext.Object);

      var cliente =  await clientesServico.BuscaPorId(1);
      
      //esta busca por id não pode vir nullo ser vier nullo não passa no teste 
      Assert.IsNotNull(cliente);     

    }
}

   