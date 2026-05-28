using AspNetCore.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Data
{
    public class AlunoRepository
    {
        private readonly ApplicationDbContext _context;

        public AlunoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Aluno>> ObterTodosAsync()
        {
            return await _context.Alunos
                .Include(a => a.Disciplinas)
                .OrderBy(a => a.Nome)
                .ToListAsync();
        }

        public async Task<Aluno?> ObterPorIdAsync(int id)
        {
            return await _context.Alunos
                .Include(a => a.Disciplinas)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Disciplina>> ObterDisciplinasAsync()
        {
            return await _context.Disciplinas
                .OrderBy(d => d.Nome)
                .ToListAsync();
        }

        public async Task AdicionarAsync(Aluno aluno, IEnumerable<int>? disciplinaIds = null)
        {
            if (string.IsNullOrWhiteSpace(aluno.Matricula))
            {
                var totalAlunos = await _context.Alunos.CountAsync();
                aluno.Matricula = $"{DateTime.Now.Year}{totalAlunos + 1:D4}";
            }

            await AtualizarDisciplinasAsync(aluno, disciplinaIds);
            await _context.Alunos.AddAsync(aluno);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Aluno aluno, IEnumerable<int>? disciplinaIds = null)
        {
            var alunoDb = await ObterPorIdAsync(aluno.Id);
            if (alunoDb == null)
            {
                return;
            }

            alunoDb.Nome = aluno.Nome;
            alunoDb.Sobrenome = aluno.Sobrenome;
            alunoDb.Cpf = aluno.Cpf;
            alunoDb.Email = aluno.Email;
            alunoDb.DataNascimento = aluno.DataNascimento;
            alunoDb.Matricula = aluno.Matricula;
            alunoDb.Curso = aluno.Curso;

            await AtualizarDisciplinasAsync(alunoDb, disciplinaIds);
            await _context.SaveChangesAsync();
        }

        public async Task ExcluirAsync(int id)
        {
            var aluno = await ObterPorIdAsync(id);
            if (aluno == null)
            {
                return;
            }

            _context.Alunos.Remove(aluno);
            await _context.SaveChangesAsync();
        }

        private async Task AtualizarDisciplinasAsync(Aluno aluno, IEnumerable<int>? disciplinaIds)
        {
            aluno.Disciplinas.Clear();

            if (disciplinaIds == null)
            {
                return;
            }

            var ids = disciplinaIds.Distinct().ToList();
            if (!ids.Any())
            {
                return;
            }

            var disciplinas = await _context.Disciplinas
                .Where(d => ids.Contains(d.Id))
                .ToListAsync();

            aluno.Disciplinas.AddRange(disciplinas);
        }
    }
}
