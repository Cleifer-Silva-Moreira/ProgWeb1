using System.ComponentModel.DataAnnotations;

namespace AspNetCore.Models
{
    public class Professor : Pessoa
    {
        [Required(ErrorMessage = "Informe o SIAP")]
        public string Siap { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe a area")]
        public string Area { get; set; } = string.Empty;
    }
}
