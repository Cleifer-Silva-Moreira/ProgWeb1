using System.ComponentModel.DataAnnotations;

namespace AspNetCore.Models
{
    public class Disciplina
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Informe o nome")]
        public string Nome { get; set; } = string.Empty;

        [Range(1, 500, ErrorMessage = "Informe uma carga horaria valida")]
        public int CargaHoraria { get; set; }

        [Required(ErrorMessage = "Informe o periodo")]
        public string Periodo { get; set; } = string.Empty;

        public int? ProfessorId { get; set; }
        public Professor? Professor { get; set; }

        public List<Aluno> Alunos { get; set; } = new();
        public List<Nota> Notas { get; set; } = new();
    }
}
