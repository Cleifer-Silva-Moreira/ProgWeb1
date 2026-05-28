using AspNetCore.Models;

namespace AspNetCore.Data
{
    public interface IProfessorRepository
    {
        Task<List<Professor>> ObterTodosAsync();
        Task<Professor?> ObterPorIdAsync(int id);
        Task AdicionarAsync(Professor professor);
        Task AtualizarAsync(Professor professor);
        Task ExcluirAsync(int id);
    }
}
