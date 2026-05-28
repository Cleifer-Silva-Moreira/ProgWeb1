using AspNetCore.Data;
using AspNetCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspNetCore.Controllers
{
    [AutenticacaoSession]
    public class DisciplinaController : Controller
    {
        private readonly DisciplinaRepository _disciplinaRepository;
        private readonly ProfessorRepository _professorRepository;

        public DisciplinaController(DisciplinaRepository disciplinaRepository, ProfessorRepository professorRepository)
        {
            _disciplinaRepository = disciplinaRepository;
            _professorRepository = professorRepository;
        }

        public async Task<IActionResult> Index()
        {
            var disciplinas = await _disciplinaRepository.ObterTodosAsync();
            return View(disciplinas);
        }

        public async Task<IActionResult> Create()
        {
            var viewModel = new DisciplinaViewModel
            {
                Professores = await ObterProfessoresSelectListAsync()
            };
            await CarregarPeriodosAsync();

            return View(viewModel);
        }

        public Task<IActionResult> CreateDisciplinaAsync()
        {
            return Create();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DisciplinaViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Professores = await ObterProfessoresSelectListAsync(viewModel.ProfessorId);
                await CarregarPeriodosAsync(viewModel.Periodo);
                return View(viewModel);
            }

            var disciplina = new Disciplina
            {
                Nome = viewModel.Nome,
                Periodo = viewModel.Periodo,
                CargaHoraria = viewModel.CargaHoraria,
                ProfessorId = viewModel.ProfessorId
            };

            await _disciplinaRepository.AdicionarAsync(disciplina);

            TempData["Tipo"] = "success";
            TempData["Mensagem"] = $"Disciplina {disciplina.Nome} cadastrada com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var disciplina = await _disciplinaRepository.ObterPorIdAsync(id);
            if (disciplina == null)
            {
                return NotFound();
            }

            await CarregarProfessoresAsync(disciplina.ProfessorId);
            await CarregarPeriodosAsync(disciplina.Periodo);
            return View(disciplina);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Disciplina disciplina)
        {
            if (id != disciplina.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                await CarregarProfessoresAsync(disciplina.ProfessorId);
                await CarregarPeriodosAsync(disciplina.Periodo);
                return View(disciplina);
            }

            await _disciplinaRepository.AtualizarAsync(disciplina);

            TempData["Tipo"] = "success";
            TempData["Mensagem"] = $"Disciplina {disciplina.Nome} atualizada com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var disciplina = await _disciplinaRepository.ObterPorIdAsync(id);

            if (disciplina == null)
            {
                return NotFound();
            }

            return View(disciplina);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _disciplinaRepository.ExcluirAsync(id);

            TempData["Tipo"] = "success";
            TempData["Mensagem"] = "Disciplina excluida com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        private async Task CarregarProfessoresAsync(int? professorId = null)
        {
            ViewBag.Professores = await ObterProfessoresSelectListAsync(professorId);
        }

        private async Task<IEnumerable<SelectListItem>> ObterProfessoresSelectListAsync(int? professorId = null)
        {
            var professores = await _professorRepository.ObterTodosAsync();

            return professores
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Nome + " " + p.Sobrenome,
                    Selected = professorId.HasValue && p.Id == professorId.Value
                });
        }

        private async Task CarregarPeriodosAsync(string? periodoSelecionado = null)
        {
            var periodos = await _disciplinaRepository.ObterPeriodosAsync();

            if (!string.IsNullOrWhiteSpace(periodoSelecionado) && !periodos.Contains(periodoSelecionado))
            {
                periodos.Add(periodoSelecionado);
            }

            ViewBag.Periodos = new SelectList(periodos.OrderBy(p => p), periodoSelecionado);
        }
    }
}
