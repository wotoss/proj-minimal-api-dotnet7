

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using proj_minimal_api_dotnet7.Models;

namespace proj_minimal_api_dotnet7.Servicos;

//estou usando a classe TokenServico como (struct),
// porque não terá construtor e á mais leve doque o (redord e class)
//para memoria..  
public struct TokenServico
{
  //= default! não pode ser nulo
  public static string Secret { get; set; } = default!;

  public static string Gerar(Administrador administrador)
  {
    //neste momento ele gera o (jwt-handler)
     var tokenHandler = new JwtSecurityTokenHandler();

     var key = Encoding.ASCII.GetBytes(Secret);
     //vou pegar as informações do (administrador e a permissão)
     var tokenDescriptor = new SecurityTokenDescriptor()
     {
        Subject = new ClaimsIdentity(new Claim[] {
          new Claim(ClaimTypes.Name, administrador.Email),
          new Claim(ClaimTypes.Role, administrador.Permissao)
        }),
        //vou colocar o token para inspirar de (3) em (3) horas
        Expires = DateTime.UtcNow.AddHours(3),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
     };
     var token = tokenHandler.CreateToken(tokenDescriptor);
     return tokenHandler.WriteToken(token);
  }

}