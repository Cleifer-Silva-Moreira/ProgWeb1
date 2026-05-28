using AspNetCore.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Data
{
    public class NotaRepository
    {
        private readonly ApplicationDbContext _context;

        public NotaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Nota>> ObterTodosAsync()
        {
            return await _context.Notas
                .Include(n => n.Aluno)
                .Include(n => n.Disciplina)
                .OrderBy(n => n.Aluno!.Nome)
                .ThenBy(n => n.Disciplina!.Nome)
                .ToListAsync();
        }

        public async Task<Nota?> ObterPorIdAsync(int id)
        {
            return await _context.Notas.FindAsync(id);
        }

        public async Task<Nota?> ObterPorIdComRelacionamentosAsync(int id)
        {
            return await _context.Notas
                .Include(n => n.Aluno)
                .Include(n => n.Disciplina)
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task AdicionarAsync(Nota nota)
        {
            await _context.Notas.AddAsync(nota);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Nota nota)
        {
            _context.Notas.Update(nota);
            await _context.SaveChangesAsync();
        }

        public async Task ExcluirAsync(int id)
        {
            var nota = await ObterPorIdAsync(id);
            if (nota == null)
            {
                return;
            }

            _context.Notas.Remove(nota);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Aluno>> ObterAlunosAsync()
        {
            return await _context.Alunos
                .OrderBy(a => a.Nome)
                .ToListAsync();
        }

        public async Task<List<Disciplina>> ObterDisciplinasAsync()
        {
            return await _context.Disciplinas
                .OrderBy(d => d.Nome)
                .ToListAsync();
        }
    }
}
