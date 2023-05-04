
using Microsoft.EntityFrameworkCore;
using proj_minimal_api_dotnet7.Models;


namespace proj_minimal_api_dotnet7.Infraestrutura.Database;

//aqui eu inicio a classe de contexto
//melhor documentação para trabalhar com entity-frameork
//https://www.entityframeworktutorial.net/efcore/entity-framework-core-dbcontext.aspx

public class DbContexto : DbContext
{
  public DbContexto (){}

  /*
   *1º passo o meu proprio de DbContexto para o meu construtor 
   *2º para ele entender que DbContextOptions<DbContexto> é em cima do meu objeto <DbContexto>
    public class DbContexto : DbContext
   *3º ele vai passar options para classe (base) ou seja para minha herança (DbContext)
  */
  public DbContexto(DbContextOptions<DbContexto> options) : base(options){}

   protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      /*
       * 1º estou trazendo lá da minha (Classe Startup) o método (StrConexao())
       * 2º Lembrando que para trafegar este método static eu preciso utilizar Classe (Startup.StrConexao());
      */
      var conexao = Startup.StrConexao();
      optionsBuilder.UseSqlServer(conexao);      
    }


  //para mapear os meus modelos e criar tabela no banco de dados
  //quando eu quero que uma propriedade minha não seja nulla (= default!)

  //adicionando o virtual esta propriedade fica (override pode ser subescrita)
  public virtual DbSet<Cliente> Clientes { get; set;} = default!;
}