
using Microsoft.EntityFrameworkCore;
using proj_minimal_api_dotnet7.Infraestrutura.Database;
using proj_minimal_api_dotnet7.Infraestrutura.Interfaces;
using proj_minimal_api_dotnet7.Models;


namespace proj_minimal_api_dotnet7.Servicos;

public class AdministradoresServico : IBancoDeDadosServico<Administrador>
{

  public AdministradoresServico(){}
  /*
    1º inicio passando o meu contexto para o meu serviço
    2º fazendo a injeção de debendencia no meu contrutor 
    3º desta forma eu tenho o contexto disponivel
     para usar nos métodos da classe de serviço => ClientesServico
    
    4º poderia trabalhar com (classes abstratas) tambem. Para herdar 
  */
  public AdministradoresServico(DbContexto dbContexto)
  {
     this.dbContexto = dbContexto;
  }
  private DbContexto dbContexto = default!;


  public virtual async Task<Administrador?> Login(string email, string senha)
  {
    return await Task.FromResult(
        this.dbContexto.Administrador
        .Where(a => a.Email == email && a.Senha == senha)
        .First()
    );
  }
  
  /*
    neste momento eu acrecentei o virtual 
    mostrando que esta classe pode ser override (sobescrita)
    * fiz isto (virtual) devido ao teste de moq.
    *mas o método continua com as mesmas funcionalidade 
    "apenas com algo a mais que é o virtual"
  */
  public virtual async Task Salvar(Administrador administrador)
  {
    //Se cliente.Id for igual a 0 => eu Adiciono na bd
    //SeNão (quero dizer se o Client.Id for != de zero) eu faço update bd
    //lembrando que como é só um if e else eu não preciso abrir o bloco {}
      if (administrador.Id == 0)
      this.dbContexto.Administrador.Add(administrador);
      else
      this.dbContexto.Administrador.Update(administrador);

      var ret = this.dbContexto.SaveChanges();
      if(ret != 1)
       throw new Exception("Não foi possível salvar o dado no banco");
      await Task.FromResult(ret);
     
     //o paralelismo (assincrono) fica por conta do FromResult
     //await Task.FromResult(this.dbContexto.SaveChanges()); 
  }

  public async Task ExcluirPorId(int id)
  {
    //busco o objeto 
    //removo o objeto
    var administrador = await this.dbContexto.Administrador.Where(c => c.Id == id).FirstAsync();
    //este é um recurso do dotne7 se estivesse utilizando dotnet6 seria !=
    //se ele não for nulo eu posso remover
    if (administrador is not null) 
    {
      await Excluir(administrador);
    }
  }

  

  public async Task Excluir(Administrador administrador)
  {
    /*
    com este excluir eu melhoro a performace 
    eu não preciso buscar por id na base de dados 
    eu excluo direto o objeto
    */
      this.dbContexto.Administrador.Remove(administrador);
      await this.dbContexto.SaveChangesAsync();
    
  }

  public async Task<Administrador?> BuscaPorId(int id)
  {
    /* 1º estava dando erro quando era nulo com o First()
     * 2º para corrigir este retorno, coloquei FirstOrDefaultAsync, ele dá um retorno com null
     * 3º e tambem passei as (?).
    */
    Administrador? administrador = await Task.FromResult(this.dbContexto.Administrador.Where(c => c.Id == id).FirstOrDefault());
    return administrador;
  }


  //este buscar todos uma boa pratica e trazer ele paginado.
  //aqui estamos trazendo todos se tivermos muitos na bd prejudic a performace.
  public async Task<List<Administrador>> Todos()
  {
    //return await this.dbContexto.Clientes.ToListAsync();
    /*
     se eu quero usar o paralelismo assincrono neste caso dos (teste)
     é melhor utilizar o Task.FromResult

     desta maneira tambem é Assincrono mas neste caso da erro no teste
     return await this.dbContexto.Clientes.ToListAsync();
    */
    return await Task.FromResult(this.dbContexto.Administrador.ToList());
  }

  public async Task Update(Administrador administradorPara, object administradorDe)
  {
    if(administradorPara.Id == 0)
    throw new Exception("Id de cliente é obrigatório");
    //pegando todas as propriedade do objeto clienteDe
    //eu vou colocar no clientePara
    foreach(var propDe in administradorDe.GetType().GetProperties())
    {
       var propPara = administradorPara.GetType().GetProperty(propDe.Name);
       if(propPara is not null)
       {
          propPara.SetValue(administradorPara, propDe.GetValue(administradorDe));
       }
    }
    this.dbContexto.Administrador.Update(administradorPara);
    await this.dbContexto.SaveChangesAsync();
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