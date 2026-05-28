using AspNetCore.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Data
{
    public class ProfessorRepository
    {
        private readonly ApplicationDbContext _context;

        public ProfessorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Professor>> ObterTodosAsync()
        {
            return await _context.Professores
                .OrderBy(p => p.Nome)
                .ToListAsync();
        }

        public async Task<Professor?> ObterPorIdAsync(int id)
        {
            return await _context.Professores.FindAsync(id);
        }

        public async Task AdicionarAsync(Professor professor)
        {
            await _context.Professores.AddAsync(professor);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Professor professor)
        {
            _context.Professores.Update(professor);
            await _context.SaveChangesAsync();
        }

        public async Task ExcluirAsync(int id)
        {
            var professor = await ObterPorIdAsync(id);
            if (professor == null)
            {
                return;
            }

            _context.Professores.Remove(professor);
            await _context.SaveChangesAsync();
        }
    }
}
