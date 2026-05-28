using System.ComponentModel.DataAnnotations;

namespace AspNetCore.Models
{
    public class Nota
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Selecione o aluno")]
        public int AlunoId { get; set; }
        public Aluno? Aluno { get; set; }

        [Required(ErrorMessage = "Selecione a disciplina")]
        public int DisciplinaId { get; set; }
        public Disciplina? Disciplina { get; set; }

        [Range(0, 10, ErrorMessage = "A nota deve estar entre 0 e 10")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "Informe a descricao")]
        public string Descricao { get; set; } = string.Empty;
    }
}
