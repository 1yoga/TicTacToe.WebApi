using TicTacToe.WebApi.Models;

namespace TicTacToe.WebApi.Repositories
{
    public interface IPlayerRepository
    {
        Task<Player> GetByIdAsync(int id);
        Task<Player> GetByNameAsync(string name);
        Task<List<Player>> GetAllAsync();
        Task<Player> CreateAsync(Player player);
        Task<Player> UpdateAsync(Player player);
        Task DeleteAsync(int id);
    }
}
