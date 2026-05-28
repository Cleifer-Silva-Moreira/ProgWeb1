using System.ComponentModel.DataAnnotations;

namespace AspNetCore.Models
{
    public class Aluno : Pessoa
    {
        public string Matricula { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe o curso")]
        public string Curso { get; set; } = string.Empty;

        public List<Disciplina> Disciplinas { get; set; } = new();
        public List<Nota> Notas { get; set; } = new();
    }
}
