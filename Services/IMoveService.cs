using TicTacToe.WebApi.Models;

namespace TicTacToe.WebApi.Services
{
    public interface IMoveService
    {
        Task<Move> GetByIdAsync(int id);
        Task<List<Move>> GetAllByGameIdAsync(int gameId);
        Task<Game> CreateAsync(int gameId, int playerId, int cell);
        Task DeleteAsync(int id);
    }
}
