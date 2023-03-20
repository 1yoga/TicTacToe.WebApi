using Microsoft.AspNetCore.Mvc;
using TicTacToe.WebApi.Models;
using TicTacToe.WebApi.Services;

namespace TicTacToe.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoveController : ControllerBase
    {
        private readonly IMoveService _moveService;
        private readonly IPlayerService _playerService;

        public MoveController(IMoveService moveService, IPlayerService playerService)
        {
            _moveService = moveService;
            _playerService = playerService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMove(int gameId, int playerId, int cell)
        {
            var player = await _playerService.GetPlayerByIdAsync(playerId);
            if (player == null)
            {
                return NotFound($"Player with ID {playerId} was not found");
            }

            var move = await _moveService.CreateAsync(gameId, playerId, cell);
            return CreatedAtAction(nameof(GetMove), new { id = move.Id }, move);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMove(int id)
        {
            var move = await _moveService.GetByIdAsync(id);
            if (move == null)
            {
                return NotFound($"Move with ID {id} was not found");
            }

            return Ok(move);
        }

        [HttpGet("game/{gameId:int}")]
        public async Task<ActionResult<List<Move>>> GetMovesByGameId(int gameId)
        {
            var moves = await _moveService.GetAllByGameIdAsync(gameId);
            if (moves == null)
            {
                return NotFound();
            }

            return moves;
        }
    }
}
