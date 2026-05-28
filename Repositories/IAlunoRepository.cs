using AspNetCore.Models;

namespace AspNetCore.Data
{
    public interface IAlunoRepository
    {
        Task<List<Aluno>> ObterTodosAsync();
        Task<Aluno?> ObterPorIdAsync(int id);
        Task AdicionarAsync(Aluno aluno);
        Task AtualizarAsync(Aluno aluno);
        Task ExcluirAsync(int id);
    }
}
