using AspNetCore.Data;
using AspNetCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspNetCore.Controllers
{
    [AutenticacaoSession]
    public class AlunoController : Controller
    {
        private readonly AlunoRepository _alunoRepository;

        public AlunoController(AlunoRepository alunoRepository)
        {
            _alunoRepository = alunoRepository;
        }

        public async Task<IActionResult> Index()
        {
            var alunos = await _alunoRepository.ObterTodosAsync();
            return View(alunos);
        }

        public async Task<IActionResult> Create()
        {
            await CarregarCursosAsync();
            await CarregarDisciplinasAsync();
            return View(new Aluno());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Aluno aluno, int[] disciplinaIds)
        {
            if (!ModelState.IsValid)
            {
                await CarregarCursosAsync(aluno.Curso);
                await CarregarDisciplinasAsync(disciplinaIds);
                return View(aluno);
            }

            await _alunoRepository.AdicionarAsync(aluno, disciplinaIds);
            TempData["Tipo"] = "success";
            TempData["Mensagem"] = $"Aluno {aluno.Nome} cadastrado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var aluno = await _alunoRepository.ObterPorIdAsync(id);
            if (aluno == null)
            {
                return NotFound();
            }

            await CarregarCursosAsync(aluno.Curso);
            await CarregarDisciplinasAsync(aluno.Disciplinas.Select(d => d.Id));
            return View(aluno);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Aluno aluno, int[] disciplinaIds)
        {
            if (id != aluno.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                await CarregarCursosAsync(aluno.Curso);
                await CarregarDisciplinasAsync(disciplinaIds);
                return View(aluno);
            }

            await _alunoRepository.AtualizarAsync(aluno, disciplinaIds);
            TempData["Tipo"] = "success";
            TempData["Mensagem"] = $"Aluno {aluno.Nome} atualizado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var aluno = await _alunoRepository.ObterPorIdAsync(id);
            if (aluno == null)
            {
                return NotFound();
            }

            return View(aluno);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _alunoRepository.ExcluirAsync(id);
            TempData["Tipo"] = "success";
            TempData["Mensagem"] = "Aluno excluido com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        private async Task CarregarCursosAsync(string? cursoSelecionado = null)
        {
            var cursos = (await _alunoRepository.ObterTodosAsync())
                .Select(a => a.Curso)
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            if (!string.IsNullOrWhiteSpace(cursoSelecionado) && !cursos.Contains(cursoSelecionado))
            {
                cursos.Add(cursoSelecionado);
            }

            ViewBag.Cursos = new SelectList(cursos.OrderBy(c => c), cursoSelecionado);
        }

        private async Task CarregarDisciplinasAsync(IEnumerable<int>? disciplinasSelecionadas = null)
        {
            var disciplinas = await _alunoRepository.ObterDisciplinasAsync();
            ViewBag.Disciplinas = new MultiSelectList(
                disciplinas,
                "Id",
                "Nome",
                disciplinasSelecionadas ?? Enumerable.Empty<int>());
        }
    }
}
