using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Linq;
using TicTacToe.WebApi.Models;
using TicTacToe.WebApi.Models.Enums;
using TicTacToe.WebApi.Repositories;

namespace TicTacToe.WebApi.Services
{
    public class MoveService : IMoveService
    {
        private readonly IMoveRepository _moveRepository;
        private readonly IGameRepository _gameRepository;
        private readonly IPlayerRepository _playerRepository;

        public MoveService(IMoveRepository moveRepository, IGameRepository gameRepository, IPlayerRepository playerRepository)
        {
            _moveRepository = moveRepository;
            _gameRepository = gameRepository;
            _playerRepository = playerRepository;
        }

        public async Task<Move> GetByIdAsync(int id)
        {
            return await _moveRepository.GetByIdAsync(id);
        }

        public async Task<List<Move>> GetAllByGameIdAsync(int gameId)
        {
            return await _moveRepository.GetAllByGameIdAsync(gameId);
        }

        public async Task<Move> CreateAsync(int gameId, int playerId, int cell, string symbol)
        {
            var game = await _gameRepository.GetByIdAsync(gameId);

            if (game == null)
            {
                throw new ApplicationException($"Game with id {gameId} was not found.");
            }

            if (game.IsDraw)
            {
                throw new ApplicationException($"Game with id {gameId} ended in a draw.");
            }

            if (game.WinnerId != null)
            {
                var player = await _playerRepository.GetByIdAsync(playerId);
                throw new ApplicationException($"Game with id {gameId} ended with the victory of the player {player.Name}.");
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

            var board = game.Board.Remove(cell, 1).Insert(cell, symbol);
            game.Board = board;

            var result = CheckResult(board);

            if(result == Result.Player1IsWin)
            {
                game.WinnerId = game.Player1Id;
            }

            if (result == Result.Player2IsWin)
            {
                game.WinnerId = game.Player2Id;
            }

            if (result == Result.IsDraw)
            {
                game.IsDraw = true;
            }

            await _moveRepository.CreateAsync(move);
            await _gameRepository.UpdateAsync(game);

            return move;
        }
        
        private Result CheckResult(string board)
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
                if(!boardArray.Contains(" "))
                {
                    return Result.IsDraw;
                }

            }
            return result;
        }
    }
}
