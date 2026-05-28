using AspNetCore.Data;
using AspNetCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspNetCore.Controllers
{
    [AutenticacaoSession]
    public class NotaController : Controller
    {
        private readonly NotaRepository _notaRepository;

        public NotaController(NotaRepository notaRepository)
        {
            _notaRepository = notaRepository;
        }

        public async Task<IActionResult> Index()
        {
            var notas = await _notaRepository.ObterTodosAsync();
            return View(notas);
        }

        public async Task<IActionResult> Create()
        {
            await CarregarCombosAsync();
            return View(new Nota());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Nota nota)
        {
            if (!ModelState.IsValid)
            {
                await CarregarCombosAsync(nota.AlunoId, nota.DisciplinaId);
                return View(nota);
            }

            await _notaRepository.AdicionarAsync(nota);
            TempData["Tipo"] = "success";
            TempData["Mensagem"] = "Nota cadastrada com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var nota = await _notaRepository.ObterPorIdAsync(id);
            if (nota == null)
            {
                return NotFound();
            }

            await CarregarCombosAsync(nota.AlunoId, nota.DisciplinaId);
            return View(nota);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Nota nota)
        {
            if (id != nota.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                await CarregarCombosAsync(nota.AlunoId, nota.DisciplinaId);
                return View(nota);
            }

            await _notaRepository.AtualizarAsync(nota);
            TempData["Tipo"] = "success";
            TempData["Mensagem"] = "Nota atualizada com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var nota = await _notaRepository.ObterPorIdComRelacionamentosAsync(id);

            if (nota == null)
            {
                return NotFound();
            }

            return View(nota);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _notaRepository.ExcluirAsync(id);

            TempData["Tipo"] = "success";
            TempData["Mensagem"] = "Nota excluida com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        private async Task CarregarCombosAsync(int? alunoId = null, int? disciplinaId = null)
        {
            var alunos = await _notaRepository.ObterAlunosAsync();
            var disciplinas = await _notaRepository.ObterDisciplinasAsync();

            ViewBag.Alunos = new SelectList(alunos, "Id", "Nome", alunoId);
            ViewBag.Disciplinas = new SelectList(disciplinas, "Id", "Nome", disciplinaId);
        }
    }
}
