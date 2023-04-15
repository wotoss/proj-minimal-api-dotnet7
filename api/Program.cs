//namespace api2;
using  proj_minimal_api_dotnet7;

//public class Program
//{
  //quando eu startar minha aplicação este vai procurar pelo 
  //método main que tem CreateHostBuilder
  // public static void Main(string[] args)
  // {
  //   //vou utilizar os argumentos que eu recebo para instartar o meu servidor web vps
  //   //vps = Servidor Virtual Privado
  //    CreateHostBuilder(args).Build().Run();
     
  // }
  //este => CreateHostBuilder vai criar um servidor web.
  
   IHostBuilder CreateHostBuilder(string[] args)
  {
    return Host.CreateDefaultBuilder(args)
      .ConfigureWebHostDefaults(webBuilder =>
      {
          webBuilder.UseStartup<Startup>();
      });
  }
  CreateHostBuilder(args).Build().Run();
