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

        [HttpDelete("{id}")]
        public async Task<ActionResult<Move>> DeleteMove(int id)
        {

            var moves = await _moveService.GetAllByGameIdAsync(id);

            if (moves == null)
            {
                return NotFound();
            }

            await _moveService.DeleteAsync(id);

            return NoContent();
        }
    }
}
