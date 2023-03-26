using TicTacToe.WebApi.Models;

namespace TicTacToe.WebApi.Services
{
    public interface IGameService
    {
        Task<List<Game>> GetAllAsync();
        Task<Game> GetByIdAsync(int id);
        Task<Game> CreateAsync(int firstPlayerId, int secondPlayerId);
        Task<Game> CreateMoveAsync(int gameId, int playerId, int cell);
        Task<Game> UpdateAsync(Game game);
        Task DeleteAsync(int id);
    }
}
