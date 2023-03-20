using TicTacToe.WebApi.Models;
using TicTacToe.WebApi.Repositories;

namespace TicTacToe.WebApi.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        private readonly IPlayerRepository _playerRepository;

        public GameService(IGameRepository gameRepository, IPlayerRepository playerRepository)
        {
            _gameRepository = gameRepository;
            _playerRepository = playerRepository;
        }

        public async Task<IEnumerable<Game>> GetAllAsync()
        {
            return await _gameRepository.GetAllAsync();
        }

        public async Task<Game> GetByIdAsync(int id)
        {
            return await _gameRepository.GetByIdAsync(id);
        }

        public async Task<Game> CreateAsync(string player1Name, string player2Name)
        {
            var player1 = await _playerRepository.CreateAsync(new Player { Name = player1Name, Symbol = "X" });
            var player2 = await _playerRepository.CreateAsync(new Player { Name = player2Name, Symbol = "O"});

            var game = new Game
            {
                Board = "         ",
                Player1Id = player1.Id,
                Player1 = player1,
                Player2Id = player2.Id,
                Player2 = player2,
                IsDraw = false
            };

            return await _gameRepository.CreateAsync(game);
        }

        public async Task<Game> UpdateAsync(Game game)
        {
            return await _gameRepository.UpdateAsync(game);
        }

        public async Task DeleteAsync(int id)
        {
            await _gameRepository.DeleteAsync(id);
        }
    }
}
