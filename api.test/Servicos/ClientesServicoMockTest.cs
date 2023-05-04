
using Api.Test.Helpers;
using Moq;
using proj_minimal_api_dotnet7.Infraestrutura.Database;
using proj_minimal_api_dotnet7.Models;
using proj_minimal_api_dotnet7.Servicos;

namespace api.test.Servico;
/*
  AGORA VAMOS FAZER O TESTE UTILIZANDO MOCK NÃO BASE DE DADOS
  NESTE MOMENTO EU INSTALEI NUGET MOQ
  dotnet add package Moq --version 4.18.4
*/
[TestClass]
public class ClientesServicoMockTest
{

    [TestMethod]
    public async Task TestaSalvarDadoNoBanco()
    {
       /*
       1º PASSO
          neste momento estou usando a (lib - class moq)
          buscando e utilizando tudo que tem em minha classe de serviço.
          * mas sem utilizar o entity-framework - sem ir a base de dados
          recebo nesta var mockClientesServico
       */
       /*
          neste teste da erro por causa do parametro
          na classe ClientesServiço public ClientesServico(DbContexto dbContexto)

          eu resolvi fazendo mais um null sem parametro 
          public ClientesServico(){}

          Isto foi feito na classe de ClientesServiço, para passar por aqui 
          sem utilizar o contexto
       */
        var mockClientesServico = new Mock<ClientesServico>();
        
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
        var clienteMock = cliente;
        clienteMock.Id = 1;
        
        /*
          4º PASSO
          este (setup) é da minha lib-moq eu faço a lambda mockClientesServico.Setup(s => s.Salvar(cliente));
          para utilizar o meu Salvar
        */
        mockClientesServico.Setup(s => s.Salvar(cliente)).Returns(Task.FromResult(cliente));
        
        await mockClientesServico.Object.Salvar(cliente);

        Assert.AreEqual(1, cliente.Id);

    }
}

   