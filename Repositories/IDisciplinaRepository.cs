using AspNetCore.Models;
namespace AspNetCore.Data
{
    public interface IDisciplinaRepository
    {
        Task<List<Disciplina>> ObterTodosAsync();
        Task<Disciplina?> ObterPorIdAsync(int id);
        Task AdicionarAsync(Disciplina disciplina);
        Task AtualizarAsync(Disciplina disciplina);
        Task ExcluirAsync(int id);
    }
}
