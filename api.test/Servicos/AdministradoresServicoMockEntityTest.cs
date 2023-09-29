
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
public class AdministradoresServicoMockEntityTest
{

    [TestMethod]
    public async Task TestaSalvar()
    {
      /*
        estou mocando a minha conexão com a base de dados
      */
       var mockContext = new Mock<DbContexto>();
       //estou mocando o DbSet<Administrador>
       var dbSetAdministradores = new Mock<DbSet<Administrador>>();

       mockContext.Setup(c => c.Administrador).Returns(dbSetAdministradores.Object);
       mockContext.Setup(c => c.SaveChanges()).Returns(1);
        
        /*
          quando eu faço esta criação não é mais o new Mock<DbContexto>(); original
          é o (DbContexto) mocado =>  new AdministradoresServico(mockContext.Object);
        */
       var AdministradoresServico = new AdministradoresServico(mockContext.Object);
        
        /*
          2º PASSO
          utilizando a minha classe mocada
          *instanciando a classe e pasando os dados para salvar no banco
        */
        var administrador = new Administrador()
        {
        
          Nome = "Usuario Teste",
          Email = "usuario@teste.com",
          //permnissão seria a regra 
          Permissao = "administrador",      
        };

        await AdministradoresServico.Salvar(administrador);
    }

    [TestMethod]
    public async Task TestaTodos()
    {
      /*
        quero que retorne uma lista com 3 Administradores
        ou seja retorne uma lista
      */
      var lista = new List<Administrador>
      {
        new Administrador { Id = 1, Nome = "BBB" },
        new Administrador { Id = 2, Nome = "ZZZ" },
        new Administrador { Id = 3, Nome = "AAA" },
      };
      var data = lista.AsQueryable();

      var administradoresServico = new AdministradoresServico(contextMock(data));

      var listaRetorno =  await administradoresServico.Todos();

      Assert.AreEqual(3, listaRetorno.Count);     

    }

     [TestMethod]
    public async Task TestaLogin()
    {
      /*
        quero que retorne uma lista com 3 Administradores
        ou seja retorne uma lista
      */
      var lista = new List<Administrador>
      {
        new Administrador { Id = 1, Nome = "woto", Email = "wotoss10@gmail.com", Senha = "02289-6wa" },
        new Administrador { Id = 2, Nome = "ZZZ" },
        new Administrador { Id = 3, Nome = "AAA" },
      };
      var data = lista.AsQueryable();    
      var AdministradoresServico = new AdministradoresServico(contextMock(data));
      var adm =  await AdministradoresServico.Login("wotoss10@gmail.com", "02289-6wa");
      
      //tem que vir um usuário
      Assert.IsNotNull(adm);     

    }

    private DbContexto contextMock(IQueryable<Administrador> data)
    { 
      var mockContext = new Mock<DbContexto>();
      var mockSet = new Mock<DbSet<Administrador>>();
          //aqui eu faço a configuração da minha lista
      mockSet.As<IQueryable<Administrador>>().Setup(m => m.Provider).Returns(data.Provider);
      mockSet.As<IQueryable<Administrador>>().Setup(m => m.Expression).Returns(data.Expression);
      mockSet.As<IQueryable<Administrador>>().Setup(m => m.ElementType).Returns(data.ElementType);
      mockSet.As<IQueryable<Administrador>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator);  
      mockContext.Setup(c => c.Administrador).Returns(mockSet.Object);

      return mockContext.Object;
    }


    //TODO ajustar o teste TestaLoginInvalido()

    // [TestMethod]
    // public async Task TestaLoginInvalido()
    // {
    //   var lista = new List<Administrador>
    //   {
    //     new Administrador { Id = 1, Nome = "woto", Email = "wotoss10@gmail.com", Senha = "02289-6wa" },
    //     new Administrador { Id = 2, Nome = "ZZZ" },
    //     new Administrador { Id = 3, Nome = "AAA" },
    //   };
    //   var data = lista.AsQueryable();

    //   var administradoresServico = new AdministradoresServico(contextMock(data));

    //   var adm =  await administradoresServico.Login("erradoss10@gmail.com", "02289-6wa-errada");
      
    //   //como estou passando a senha errada ele não vai achar, o usuario.
    //   //então vira nullo
    //   Assert.IsNull(adm);     

    // }

    /*
      Teste busca por id
    */
    [TestMethod]
    public async Task TestaBuscaPorId()
    {
      /*
        buscando por id
      */
      var lista = new List<Administrador>
      {
        new Administrador { Id = 1, Nome = "livia" },
        new Administrador { Id = 2, Nome = "vania" },
        new Administrador { Id = 3, Nome = "woto" },
      };
      var data = lista.AsQueryable();

      var administradoresServico = new AdministradoresServico(contextMock(data));

      var Administrador =  await administradoresServico.BuscaPorId(1);
      
      //esta busca por id não pode vir nullo ser vier nullo não passa no teste 
      Assert.IsNotNull(Administrador);     

    }
}

   