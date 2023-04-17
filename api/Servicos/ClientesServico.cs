
using Microsoft.EntityFrameworkCore;
using proj_minimal_api_dotnet7.Infraestrutura.Database;
using proj_minimal_api_dotnet7.Infraestrutura.Interfaces;
using proj_minimal_api_dotnet7.Models;

namespace proj_minimal_api_dotnet7.Servicos;

public class ClientesServico : IBancoDeDadosServico<Cliente>
{
  /*
    1º inicio passando o meu contexto para o meu serviço
    2º fazendo a injeção de debendencia no meu contrutor 
    3º desta forma eu tenho o contexto disponivel
     para usar nos métodos da classe de serviço => ClientesServico
    
    4º poderia trabalhar com (classes abstratas) tambem. Para herdar 
  */
  public ClientesServico(DbContexto dbContexto)
  {
     this.dbContexto = dbContexto;
  }
  private DbContexto dbContexto;

  public async Task Salvar(Cliente cliente)
  {
    //Se cliente.Id for igual a 0 => eu Adiciono na bd
    //SeNão (quero dizer se o Client.Id for != de zero) eu faço update bd
    //lembrando que como é só um if e else eu não preciso abrir o bloco {}
     if (cliente.Id == 0)
     this.dbContexto.Clientes.Add(cliente);
     else
     this.dbContexto.Clientes.Update(cliente);

     await this.dbContexto.SaveChangesAsync(); 
  }

  public async Task ExcluirPorId(int id)
  {
    //busco o objeto 
    //removo o objeto
    var cliente = await this.dbContexto.Clientes.Where(c => c.Id == id).FirstAsync();
    //este é um recurso do dotne7 se estivesse utilizando dotnet6 seria !=
    //se ele não for nulo eu posso remover
    if (cliente is not null) 
    {
      await Excluir(cliente);
    }
  }

  public async Task Excluir(Cliente cliente)
  {
    /*
    com este excluir eu melhoro a performace 
    eu não preciso buscar por id na base de dados 
    eu excluo direto o objeto
    */
      this.dbContexto.Clientes.Remove(cliente);
      await this.dbContexto.SaveChangesAsync();
    
  }

  public async Task<Cliente> BuscaPorId(int id)
  {
    return await this.dbContexto.Clientes.Where(c => c.Id == id).FirstAsync();
  }


  //este buscar todos uma boa pratica e trazer ele paginado.
  //aqui estamos trazendo todos se tivermos muitos na bd prejudic a performace.
  public async Task<List<Cliente>> Todos()
  {
    return await this.dbContexto.Clientes.ToListAsync();
  }

  /*
  * 1º para fazer a implementação de todos os métodos para a base de dados
  eu não utilizo nem (viewModel e DTO)
    2º isto por que um dado sensivel como (senha, token, data especifica entre outros)
  eu acabo não passando (viewModel e DTO) por ser dados que vão tramitar nas requisições 
    3º mas quando vou pesistir os dados na banco. Ai eu preciso de todas as informações
  inclusive as sensiveis. Desta forma eu utilizo a (classe)    
  */




}