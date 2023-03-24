using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using TicTacToe.WebApi.Models;
using TicTacToe.WebApi.Models.Enums;
using TicTacToe.WebApi.Services;

namespace TicTacToe.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetPlayerById(int id)
        {
            var player = await _playerService.GetPlayerByIdAsync(id);

            if (player == null)
            {
                return NotFound();
            }

            return Ok(player);
        }

        [HttpPost]
        public async Task<ActionResult<Player>> CreatePlayer(string playerName)
        {
            var createdPlayer = await _playerService.CreatePlayerAsync(playerName);

            return Ok(createdPlayer);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Player>> UpdatePlayer(int id, Player player)
        {
            if (id != player.Id)
            {
                return BadRequest();
            }

            await _playerService.UpdatePlayerAsync(player);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Player>> DeletePlayer(int id)
        {
            var player = await _playerService.GetPlayerByIdAsync(id);

            if (player == null)
            {
                return NotFound();
            }

            await _playerService.DeletePlayerAsync(id);

            return NoContent();
        }
    }
}
