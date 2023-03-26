using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpGet]
        public async Task<ActionResult<List<Player>>> GetAllPlayers()
        {
            var players = await _playerService.GetAllPlayersAsync();
            if (players.Count < 1 )
            {
                return NotFound();
            }
            return Ok(players.ToArray());
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
        public async Task<ActionResult<Player>> UpdatePlayer(int id, string name)
        {
            var player = await _playerService.GetPlayerByIdAsync(id);

            if (player == null)
            {
                return NotFound();
            }
            player.Name = name;
            var updatedPlayer = await _playerService.UpdatePlayerAsync(player);

            return Ok(updatedPlayer);
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
