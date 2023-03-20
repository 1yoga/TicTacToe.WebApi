using TicTacToe.WebApi.Models;

namespace TicTacToe.WebApi.Services
{
    public interface IMoveService
    {
        Task<Move> GetByIdAsync(int id);
        Task<List<Move>> GetAllByGameIdAsync(int gameId);
        Task<Move> CreateAsync(int gameId, int playerId, int cell, string symbol);
    }
}
