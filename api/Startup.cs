using Microsoft.AspNetCore.Mvc;
using proj_minimal_api_dotnet7.DTOs;
using proj_minimal_api_dotnet7.Models;
using proj_minimal_api_dotnet7.ModelView;
using Microsoft.OpenApi.Models;
using proj_minimal_api_dotnet7.Infraestrutura.Database;
using Microsoft.EntityFrameworkCore;
using proj_minimal_api_dotnet7.Servicos;
using proj_minimal_api_dotnet7.Infraestrutura.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using proj_minimal_api_dotnet7.Routes;


namespace proj_minimal_api_dotnet7;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
     /*
      esta variavel (public static IConfiguration?) ela é (static)
      então eu posso utiliza-la ou compartilhar, lá no arquivo =>  (DbContextoTeste.cs) 
      Como esta só com public (public IConfiguration Configuration { get; set; })
    eu não conseguia
       Lembrando que por ele ter ficado (static) eu uso atraves da classe (Startup.Configuration)
    */
    public IConfiguration? Configuration { get; set; }
    
    public static string? StrConexao(IConfiguration? Configuration = null)
    {
        
        //vou utilizar a minha string de conexão da minha variavel de ambente 
        //se eu coloco variavel de ambiente eu tenho ue configurar o meu sistema operacional 
        //para lêr os dados.DATABASE_URL_MINIMAL_API
        
        // ======= [Dá prioridade ao appsettings] ======
        /*
          if(Configuration is not null)//recurso dotnet7 is null
          {
             return Configuration?.GetConnectionString("Conexao");
          }
          return Environment.GetEnvironmentVariable("DATABASE_URL_MINIMAL_API");
        */
        
         string? conexao = Environment.GetEnvironmentVariable("DATABASE_URL_MINIMAL_API");
         if(conexao is null)//recurso dotnet7 is null
          {
             conexao = Configuration?.GetConnectionString("Conexao");
          }
         return conexao;
    }


    /*
     Crio um método (StrConexao()) onde eu busco a StringDeConexao
     Este método é statico e eu posso compartilhar entre a aplicação
     Lembrando: e para chamar em outras partes do projeto eu utilizo a classe Startup.StrConexao
    */
    // public string? StrConexao()
    // {
    //     //vou utilizar a minha string de conexão da minha variavel de ambente 
    //     //se eu coloco variavel de ambiente eu tenho ue configurar o meu sistema operacional 
    //     //para lêr os dados.DATABASE_URL_MINIMAL_API
    //     string? conexao = Environment.GetEnvironmentVariable("DATABASE_URL_MINIMAL_API");
    //     if(conexao is null)//recurso dotnet7 is null
    //     {
    //        conexao = Configuration?.GetConnectionString("Conexao");
    //     }
    //     return conexao;
    // }
    

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        
        //estou passando o método (StrConexao()) que construi para buscar a conexão 
        var conexao = StrConexao(Configuration);
        services.AddDbContext<DbContexto>( options =>
        {//options.UseSqlServer(conexao, ServerVersion.AutoDetect(conexao));
           options.UseSqlServer(conexao);
        });
        
        /*
           desta forma eu consigo utilizar clientesServico direto em minhas rotas
           com AddScoped eu estou dizendo que quem vai resolver este
           contrato => IBancoDeDadosServico<Cliente> será este srviço =>  ClientesServico
           desta forma eu não estou trabalhando direto com a implementação ClientesServico
           e sim com o contrato => IBancoDeDadosServico<Cliente>
        */
        /*
          1º Quano eu mostro que esta é a minha interface => IBancoDeDadosServico<Cliente>
          2º e este é o meu objeto => ClientesServico
          3º e que este objeto => ClientesServico recebe por injeção no construtor este DbContexto
          4º o C# faz tudo quando eu coloco ele no => AddScoped
        */
        services.AddScoped<IBancoDeDadosServico<Cliente>, ClientesServico>();
        services.AddScoped<ILogin<Administrador>, AdministradoresServico>();

        //vamos pegar o Secret lá do appsettings(SecretJwt) atraves da (propriedade Configuration)
        //como ele pode vir nullo - caso venha eu (passo vazio) (se vinher nullo ?? "")
        TokenServico.Secret = Configuration?["SecretJwt"] ?? "";
        //esta secretJwt precisa me devolver um byte. estou transformando em byte => GetBytes
        var key = Encoding.ASCII.GetBytes(TokenServico.Secret);

        //começar configurar o shema de (jwt)
        services.AddAuthentication( x =>
        {
          //neste estou informando que vou usar (autenticação)
          x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
          //neste estou informando que vou usar (autenticação)
          x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
          //estou configurando o meu (objeto AddJwtBearer) para le ter a possibilidade 
          //de trabalhar com (AddJwtBearer)
          x.RequireHttpsMetadata = false;
          x.SaveToken = true;
          x.TokenValidationParameters = new TokenValidationParameters
          {
            ValidateIssuerSigningKey = true,
            //aqui eu faço uma criptografia symetrica da minha key
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
          };
        });
       
        services.AddAuthorization(options => 
        {
        /*
          agora vou colocar no meu serviço dois tipos de autorização
          um chamado (administrador) e outro chamado (editor)
          * Lembrando que estas duas autorização estarão lincados
          com a propriedade permissao {get; set} da classe administrador

          Desta forma eu "defino a politica de acesso" de (administrador) e (editor)
        */
          options.AddPolicy("administrador", policy => policy.RequireClaim("administrador"));
          options.AddPolicy("editor", policy => policy.RequireClaim("editor"));
        });

        //serviço de configuração do swagger
        services.AddSwaggerGen(c => 
        {
           c.SwaggerDoc("v1", new OpenApiInfo
           {
             Title = "Minimal API",
             Description = "[ Programador-Camisa-10 ]",
             Contact = new OpenApiContact { Name = "woto", Email = "wotoss10@gmail.com" },
             License = new OpenApiLicense { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
           });
           c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
           {
            Description = "Insira o token JWT como exemplo: Bearer {SEU_TOKEN}",
            Name = "Authorization",
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey
           });
           //por aqui passa o (parametro que vou enviar)
           c.AddSecurityRequirement(new OpenApiSecurityRequirement
           {
            {
              new OpenApiSecurityScheme
              {
                Reference = new OpenApiReference
                {
                  Type = ReferenceType.SecurityScheme,
                  Id = "Bearer"
                }
              },
              new string[] {}
           }
           });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        //vou deixar o meu swagger aberto, mas é sempre bom deixar em modo de desencolvimento
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseRouting();
         
        //1º primeiro eu autentico me certifico de quem esta na aplicação
        app.UseAuthentication();
        //2º segundo lugar eu faço a autorização para as telas ou ações (crud).
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            HomeRoute.MapRoutes(endpoints);
            AdministradoresRoute.MapRoutes(endpoints);
            ClientesRoute.MapRoutes(endpoints);
        });
    }

}
