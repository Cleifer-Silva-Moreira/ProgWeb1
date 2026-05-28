using AspNetCore.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Data
{
    public class DisciplinaRepository : IDisciplinaRepository
    {
        private readonly ApplicationDbContext _context;

        public DisciplinaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Disciplina>> ObterTodosAsync()
        {
            return await _context.Disciplinas
                .Include(d => d.Professor)
                .OrderBy(d => d.Nome)
                .ToListAsync();
        }

        public async Task<Disciplina?> ObterPorIdAsync(int id)
        {
            return await _context.Disciplinas
                .Include(d => d.Professor)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task AdicionarAsync(Disciplina disciplina)
        {
            await _context.Disciplinas.AddAsync(disciplina);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Disciplina disciplina)
        {
            _context.Disciplinas.Update(disciplina);
            await _context.SaveChangesAsync();
        }

        public async Task ExcluirAsync(int id)
        {
            var disciplina = await ObterPorIdAsync(id);
            if (disciplina == null)
            {
                return;
            }

            _context.Disciplinas.Remove(disciplina);
            await _context.SaveChangesAsync();
        }

        public async Task<List<string>> ObterPeriodosAsync()
        {
            return await _context.Disciplinas
                .Select(d => d.Periodo)
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .Distinct()
                .OrderBy(p => p)
                .ToListAsync();
        }
    }
}
