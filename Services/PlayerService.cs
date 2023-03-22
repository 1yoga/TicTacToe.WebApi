using TicTacToe.WebApi.Models;
using TicTacToe.WebApi.Repositories;

namespace TicTacToe.WebApi.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;

        public PlayerService(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        public async Task<Player> GetPlayerByIdAsync(int id)
        {
            return await _playerRepository.GetByIdAsync(id);
        }

        public async Task<Player> GetPlayerByNameAsync(string name)
        {
            return await _playerRepository.GetByNameAsync(name);
        }

        public async Task<List<Player>> GetAllPlayersAsync()
        {
            return await _playerRepository.GetAllAsync();
        }

        public async Task<Player> CreatePlayerAsync(string name, string symbol)
        {
            var player = new Player
            {
                Name = name,
                Symbol = symbol
            };
            return await _playerRepository.CreateAsync(player);
        }

        public async Task<Player> UpdatePlayerAsync(Player player)
        {
            return await _playerRepository.UpdateAsync(player);
        }

        public async Task DeletePlayerAsync(int id)
        {
            await _playerRepository.DeleteAsync(id);
        }
    }
}
