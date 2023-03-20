using TicTacToe.WebApi.Models;

namespace TicTacToe.WebApi.Services
{
    public interface IGameService
    {
        Task<IEnumerable<Game>> GetAllAsync();
        Task<Game> GetByIdAsync(int id);
        Task<Game> CreateAsync(string player1Name, string player2Name);
        Task<Game> UpdateAsync(Game game);
        Task DeleteAsync(Game game);
    }
}
