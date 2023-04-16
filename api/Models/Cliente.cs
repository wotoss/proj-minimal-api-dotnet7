/*
estou colocando o namespace na estrutura do C# 10 para cima
a estrutura sem ter bloco.
*/
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace proj_minimal_api_dotnet7.Models;
//namespace api2;
//poderia ser um class record ou struct 
//assim será o nome da tabela na base de dados
[Table("clientes")] 

public record Cliente 
{
   //vou passar no contrutor uma data. vou fazer isto para meu teste
   //como a data irá sempre mudar. tenho que ajustar no teste
   public Cliente ()
   {
      this.DataCriacao = DateTime.Now;
   }
   [Key]
   [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
   public int Id {get; set;}

   // (= default!;) eu não quero que minha propriedade seja nulla 
   [Required(ErrorMessage = "Nome é obrigatório")]
   [MaxLength(100)]
   public string? Nome {get; set;}
   
   /*
   * Lembrando que estes ajustes nas propriedades 
   * DataAnnotations (obrigatorio, tamanho e outros)
   * terão reflexo nos testes
   */
   [MaxLength(20)]
   [Required(ErrorMessage = "Telefone é obrigatório")]
   public string? Telefone {get; set;}
   
   [MaxLength(200)]
   public string? Email {get; set;}

   //apenas como teste 
   public DateTime DataCriacao {get; set;}

}