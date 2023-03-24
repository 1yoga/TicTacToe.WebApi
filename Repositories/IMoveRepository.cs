using TicTacToe.WebApi.Models;

namespace TicTacToe.WebApi.Repositories
{
    public interface IMoveRepository
    {
        Task<Move> GetByIdAsync(int id);
        Task<List<Move>> GetAllByGameIdAsync(int gameId);
        Task<Move> CreateAsync(Move move);
        Task UpdateAsync(Move move);
        Task DeleteAsync(int id);
    }
}
