using AspNetCore.Data;
using AspNetCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspNetCore.Controllers
{
    [AutenticacaoSession]
    public class ProfessorController : Controller
    {
        private readonly ProfessorRepository _professorRepository;

        public ProfessorController(ProfessorRepository professorRepository)
        {
            _professorRepository = professorRepository;
        }

        public async Task<IActionResult> Index()
        {
            var professores = await _professorRepository.ObterTodosAsync();
            return View(professores);
        }

        public async Task<IActionResult> Create()
        {
            await CarregarAreasAsync();
            return View(new Professor());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Professor professor)
        {
            if (!ModelState.IsValid)
            {
                await CarregarAreasAsync(professor.Area);
                return View(professor);
            }

            await _professorRepository.AdicionarAsync(professor);
            TempData["Tipo"] = "success";
            TempData["Mensagem"] = $"Professor {professor.Nome} cadastrado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var professor = await _professorRepository.ObterPorIdAsync(id);
            if (professor == null)
            {
                return NotFound();
            }

            await CarregarAreasAsync(professor.Area);
            return View(professor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Professor professor)
        {
            if (id != professor.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                await CarregarAreasAsync(professor.Area);
                return View(professor);
            }

            await _professorRepository.AtualizarAsync(professor);
            TempData["Tipo"] = "success";
            TempData["Mensagem"] = $"Professor {professor.Nome} atualizado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var professor = await _professorRepository.ObterPorIdAsync(id);
            if (professor == null)
            {
                return NotFound();
            }

            return View(professor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _professorRepository.ExcluirAsync(id);
            TempData["Tipo"] = "success";
            TempData["Mensagem"] = "Professor excluido com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        private async Task CarregarAreasAsync(string? areaSelecionada = null)
        {
            var areas = (await _professorRepository.ObterTodosAsync())
                .Select(p => p.Area)
                .Where(a => !string.IsNullOrWhiteSpace(a))
                .Distinct()
                .OrderBy(a => a)
                .ToList();

            if (!string.IsNullOrWhiteSpace(areaSelecionada) && !areas.Contains(areaSelecionada))
            {
                areas.Add(areaSelecionada);
            }

            ViewBag.Areas = new SelectList(areas.OrderBy(a => a), areaSelecionada);
        }
    }
}
