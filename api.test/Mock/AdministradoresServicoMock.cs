

using proj_minimal_api_dotnet7.Infraestrutura.Interfaces;
using proj_minimal_api_dotnet7.Models;

namespace Api.Test.Mock;
//como eu só vou utilizar esta classe aqui, não preciso usar public
public class AdministradoresServicoMock : ILogin<Administrador>
{

//adminstradorFake
public static Administrador AdministradorFake()
{
  return new Administrador
        {
          Nome = "woto",
          Email = "wotoss10@gmail.com",
          Senha = "123456",
          Permissao = "administrador"
        };
}

 public Task<Administrador?> BuscaPorId(int id)
  {
    Administrador? administrador = null;
    //Se eu passei, Id != 1 ele retorna vazio ou null
    //Caso eu passe Id == 1 eu não entro no if e retorno a instaância mocada
    if(id != 1) return Task.FromResult(administrador);

    //busca por id retorna um administrador
     administrador = new Administrador()
      {
        Id = 1,
        Nome = "UsuarioMock",
        Permissao = "administrador",
        Email = "test@mock.com"
      };
    //isto é a mesma coisa asyc await usando FromResult
    return Task.FromResult(administrador ?? null);
  }

 public Task Excluir(Administrador objeto)
  {
    return Task.FromResult(()=>{});
  }

 public Task ExcluirPorId(int id)
  {
    return Task.FromResult(()=>{});
  }

 public Task Salvar(Administrador objeto)
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

  public Task Update(Administrador administradorPara, object administradorDe)
  {
    if(administradorPara.Id == 0)
    throw new Exception("Id de administrador é obrigatório");
    //pegando todas as propriedade do objeto administradorDe
    //eu vou colocar no administradorPara
    foreach(var propDe in administradorDe.GetType().GetProperties())
    {
       var propPara = administradorPara.GetType().GetProperty(propDe.Name);
       if(propPara is not null)
       {
          propPara.SetValue(administradorPara, propPara.GetValue(administradorDe));
       }
    }
    //dando um retorno do (FromResult) vazio.
    return Task.FromResult(()=>{});
  }


 public  Task<List<Administrador>> Todos()
  {
    //vou mocar o retorno lista
    //sempre que eu usar o método todos ele vai retornar uma lista com usuario
    var lista = new List<Administrador>()
    {
        new Administrador()
        {
          Id = 1,
          Nome = "UsuarioMock",
          Permissao = "administrador",
          Email = "teste@mock.com"
        }
    };
    return Task.FromResult(lista);
  }

    public Task<Administrador?> LoginAsync(string email, string senha)
    {
        return Task.FromResult(
         AdministradorFake() ?? null
        );
    }
}