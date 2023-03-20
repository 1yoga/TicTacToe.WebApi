using Microsoft.AspNetCore.Mvc;
using TicTacToe.WebApi.Models;
using TicTacToe.WebApi.Services;

namespace TicTacToe.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetById(int id)
        {
            var game = await _gameService.GetByIdAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            return Ok(game);
        }

        [HttpPost]
        public async Task<ActionResult<Game>> Create(string player1Name, string player2Name)
        {
            var game = await _gameService.CreateAsync(player1Name, player2Name);
            return CreatedAtAction(nameof(GetById), new { id = game.Id }, game);
        }
    }
}
