using TicTacToe.WebApi.Models;
using TicTacToe.WebApi.Repositories;

namespace TicTacToe.WebApi.Services
{
    public class MoveService : IMoveService
    {
        private readonly IMoveRepository _moveRepository;
        private readonly IGameRepository _gameRepository;

        public MoveService(IMoveRepository moveRepository, IGameRepository gameRepository)
        {
            _moveRepository = moveRepository;
            _gameRepository = gameRepository;
        }

        public async Task<Move> GetByIdAsync(int id)
        {
            return await _moveRepository.GetByIdAsync(id);
        }

        public async Task<List<Move>> GetAllByGameIdAsync(int gameId)
        {
            return await _moveRepository.GetAllByGameIdAsync(gameId);
        }

        public async Task<Move> CreateAsync(int gameId, int playerId, int cell)
        {
            var game = await _gameRepository.GetByIdAsync(gameId);

            if (game == null)
            {
                throw new ApplicationException($"Game with id {gameId} was not found.");
            }

            var existingMoves = await _moveRepository.GetAllByGameIdAsync(gameId);

            if (existingMoves.Any(move => move.Cell == cell))
            {
                throw new ApplicationException($"Move with cell {cell} already exists in game board with id {gameId}.");
            }

            var move = new Move
            {
                GameId = gameId,
                PlayerId = playerId,
                Cell = cell
            };

            await _moveRepository.CreateAsync(move);
            await _gameRepository.UpdateAsync(game);

            return move;
        }
    }
}
