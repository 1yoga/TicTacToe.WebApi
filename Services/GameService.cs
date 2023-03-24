using TicTacToe.WebApi.Models;
using TicTacToe.WebApi.Models.Enums;
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

        public async Task<Game> CreateAsync(int firstPlayerId, int secondPlayerId)
        {
            var game = new Game
            {
                Board = "         ",
                FirstPlayerId = firstPlayerId,
                SecondPlayerId = secondPlayerId,
                IsDraw = false,
                Status = Status.NextTurnFirstPlayer
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
