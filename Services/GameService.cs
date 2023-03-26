using TicTacToe.WebApi.Models;
using TicTacToe.WebApi.Models.Enums;
using TicTacToe.WebApi.Repositories;

namespace TicTacToe.WebApi.Services
{
    public class GameService : IGameService
    {
        private readonly IMoveRepository _moveRepository;
        private readonly IGameRepository _gameRepository;
        private readonly IPlayerRepository _playerRepository;

        public GameService(IGameRepository gameRepository, IPlayerRepository playerRepository, IMoveRepository moveRepository)
        {
            _gameRepository = gameRepository;
            _playerRepository = playerRepository;
            _moveRepository = moveRepository;
        }

        public async Task<List<Game>> GetAllAsync()
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

        public async Task<Game> CreateMoveAsync(int gameId, int playerId, int cell)
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
            var symbol = game.Status == Status.NextTurnFirstPlayer ? Symbol.X : Symbol.O;
            var move = new Move
            {
                GameId = gameId,
                PlayerId = playerId,
                Cell = cell,
                Symbol = symbol
            };
            var board = game.Board.Remove(cell, 1).Insert(cell, symbol.ToString());
            game.Board = board;

            var result = CheckBoard(board);

            switch (result)
            {
                case Result.Player1IsWin:
                    game.WinnerId = game.FirstPlayerId;
                    game.Status = Status.GameOver;
                    break;
                case Result.Player2IsWin:
                    game.WinnerId = game.SecondPlayerId;
                    game.Status = Status.GameOver;
                    break;
                case Result.IsDraw:
                    game.IsDraw = true;
                    game.Status = Status.GameOver;
                    break;
            }
            if (game.Status != Status.GameOver)
                game.Status = game.Status == Status.NextTurnFirstPlayer ? Status.NextTurnSecondPlayer : Status.NextTurnFirstPlayer;

            await _moveRepository.CreateAsync(move);
            await _gameRepository.UpdateAsync(game);

            return game;
        }

        public async Task<Game> UpdateAsync(Game game)
        {
            return await _gameRepository.UpdateAsync(game);
        }

        public async Task DeleteAsync(int id)
        {
            await _gameRepository.DeleteAsync(id);
        }

        private Result CheckBoard(string board)
        {
            var result = Result.Continue;
            if (board != null)
            {
                string[] boardArray = new string[board.Length];

                for (int i = 0; i < board.Length; i++)
                {
                    boardArray[i] = board[i].ToString();
                }

                var winningConditions = new List<int[]>
                {
                    new int[] { 0, 1, 2 },
                    new int[] { 3, 4, 5 },
                    new int[] { 6, 7, 8 },
                    new int[] { 0, 3, 6 },
                    new int[] { 1, 4, 7 },
                    new int[] { 2, 5, 8 },
                    new int[] { 0, 4, 8 },
                    new int[] { 2, 4, 6 },
                };

                for (var i = 0; i <= 7; i++)
                {
                    var winCondition = winningConditions[i];
                    var a = boardArray[winCondition[0]];
                    var b = boardArray[winCondition[1]];
                    var c = boardArray[winCondition[2]];
                    if (a == " " || b == " " || c == " ")
                        continue;
                    if (a == b && b == c)
                    {
                        return a.ToString() == "X" ? Result.Player1IsWin : Result.Player2IsWin;
                    }
                }
                if (!boardArray.Contains(" "))
                {
                    return Result.IsDraw;
                }

            }
            return result;
        }
    }
}
