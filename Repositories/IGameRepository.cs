using TicTacToe.WebApi.Models;

namespace TicTacToe.WebApi.Repositories
{
    public interface IGameRepository
    {
        Task<IEnumerable<Game>> GetAllAsync();
        Task<Game> GetByIdAsync(int id);
        Task<Game> CreateAsync(Game game);
        Task<Game> UpdateAsync(Game game);
        Task DeleteAsync(int id);
    }
}
