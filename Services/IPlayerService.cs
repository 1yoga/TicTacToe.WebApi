using TicTacToe.WebApi.Models;

namespace TicTacToe.WebApi.Services
{
    public interface IPlayerService
    {
        Task<Player> GetPlayerByIdAsync(int id);
        Task<List<Player>> GetAllPlayersAsync();
        Task<Player> CreatePlayerAsync(string name);
        Task<Player> UpdatePlayerAsync(Player player);
        Task DeletePlayerAsync(int id);
    }
}
