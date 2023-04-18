

using proj_minimal_api_dotnet7.Infraestrutura.Interfaces;
using proj_minimal_api_dotnet7.Models;

namespace Api.Test.Mock;
//como eu só vou utilizar esta classe aqui, não preciso usar public
public class ClientesServicoMock : IBancoDeDadosServico<Cliente>
{
 public Task<Cliente?> BuscaPorId(int id)
  {
    Cliente? cliente = null;
    //Se eu passei, Id != 1 ele retorna vazio ou null
    //Caso eu passe Id == 1 eu não entro no if e retorno a instaância mocada
    if(id != 1) return Task.FromResult(cliente);

    //busca por id retorna um cliente
     cliente = new Cliente()
      {
        Id = 1,
        Nome = "UsuarioMock",
        Email = "test@mock.com"
      };
    //isto é a mesma coisa asyc await usando FromResult
    return Task.FromResult(cliente ?? null);
  }

 public Task Excluir(Cliente objeto)
  {
    return Task.FromResult(()=>{});
  }

 public Task ExcluirPorId(int id)
  {
    return Task.FromResult(()=>{});
  }

 public Task Salvar(Cliente objeto)
  {
    /*
    Só quero testar que meu controlador chame a minha classe de servico
    e a classe de serviço esta mocada.
    Fazendo isto eu evito que ele vá até ao base de dados.

    no momento de salvar ele gera o Id=1 e retorna nada.
    EndOfStreamException é o salvar do moc
    */
    objeto.Id = 1;
    //vou colocar um arrow function vazio, eu não faço nada.
    return Task.FromResult(() => {});
  }

  public Task Update(Cliente clientePara, object clienteDe)
  {
    if(clientePara.Id == 0)
    throw new Exception("Id de cliente é obrigatório");
    //pegando todas as propriedade do objeto clienteDe
    //eu vou colocar no clientePara
    foreach(var propDe in clienteDe.GetType().GetProperties())
    {
       var propPara = clientePara.GetType().GetProperty(propDe.Name);
       if(propPara is not null)
       {
          propPara.SetValue(clientePara, propPara.GetValue(clienteDe));
       }
    }
    //dando um retorno do (FromResult) vazio.
    return Task.FromResult(()=>{});
  }


 public  Task<List<Cliente>> Todos()
  {
    //vou mocar o retorno lista
    //sempre que eu usar o método todos ele vai retornar uma lista com usuario
    var lista = new List<Cliente>()
    {
        new Cliente()
        {
          Id = 1,
          Nome = "UsuarioMock",
          Telefone = "(11) 94704-7361",
          Email = "teste@mock.com"
        }
    };
    return Task.FromResult(lista);
  }
}