using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspNetCore.Models
{
    public class DisciplinaViewModel
    {
        [Required(ErrorMessage = "Informe o nome")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe o periodo")]
        public string Periodo { get; set; } = string.Empty;

        [Range(1, 500, ErrorMessage = "Informe uma carga horaria valida")]
        public int CargaHoraria { get; set; }

        public int? ProfessorId { get; set; }

        public IEnumerable<SelectListItem> Professores { get; set; } = new List<SelectListItem>();
    }
}
