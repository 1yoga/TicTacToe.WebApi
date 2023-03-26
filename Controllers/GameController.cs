using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using TicTacToe.WebApi.Models;
using TicTacToe.WebApi.Models.Enums;
using TicTacToe.WebApi.Services;

namespace TicTacToe.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly IPlayerService _playerService;

        public GameController(IGameService gameService, IPlayerService playerService)
        {
            _gameService = gameService;
            _playerService = playerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetAllGames()
        {
            var games = await _gameService.GetAllAsync();
            if (games.Count < 1)
            {
                return NotFound();
            }
            return Ok(games.ToArray());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGameById(int id)
        {
            var game = await _gameService.GetByIdAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            return Ok(game);
        }

        [HttpPost]
        public async Task<ActionResult<Game>> CreateGame(int firstPlayerId, int secondPlayerId)
        {
            var firstPlayer = await _playerService.GetPlayerByIdAsync(firstPlayerId);
            if (firstPlayer == null)
            {
                return NotFound($"Player with ID {firstPlayerId} was not found");
            }
            
            var secondPlayer = await _playerService.GetPlayerByIdAsync(secondPlayerId);
            if (secondPlayer == null)
            {
                return NotFound($"Player with ID {secondPlayerId} was not found");
            }

            if (firstPlayerId == secondPlayerId)
                return BadRequest($"Players must be different");

            var game = await _gameService.CreateAsync(firstPlayerId, secondPlayerId);
            return Ok(game);
        }

        [HttpPost("{gameId}/createMove")]
        public async Task<ActionResult> CreateMove(int gameId, int playerId, int cell)
        {
            var game = await _gameService.GetByIdAsync(gameId);
            if (game == null)
            {
                return NotFound($"Game with ID {gameId} was not found");
            }
            var player = await _playerService.GetPlayerByIdAsync(playerId);
            if (player == null)
            {
                return NotFound($"Player with ID {playerId} was not found");
            }

            switch (game.Status)
            {
                case Status.GameOver:
                    return BadRequest($"Game with ID {playerId} was over");
                case Status.NextTurnFirstPlayer when game.FirstPlayerId != playerId:
                    return BadRequest($"Player 1's turn to go");
                case Status.NextTurnSecondPlayer when game.SecondPlayerId != playerId:
                    return BadRequest($"Player 2's turn to go");
            }

            try
            {
                var request = await _gameService.CreateMoveAsync(gameId, playerId, cell);
                return Ok(request);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Game>> DeleteGame(int id)
        {
            var game = await _gameService.GetByIdAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            await _gameService.DeleteAsync(id);

            return NoContent();
        }
    }
}
