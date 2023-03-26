using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.WebApi.Models;
using TicTacToe.WebApi.Repositories;
using TicTacToe.WebApi.Services;

namespace TicTacToe.WebApi.Tests.Services
{
    public class PlayerServiceTests
    {
        private readonly Mock<IPlayerRepository> _mockPlayerRepository;
        private readonly PlayerService _playerService;

        public PlayerServiceTests()
        {
            _mockPlayerRepository = new Mock<IPlayerRepository>();
            _playerService = new PlayerService(_mockPlayerRepository.Object);
        }

        [Fact]
        public async Task GetPlayerByIdAsync_ShouldReturnCorrectPlayer()
        {
            // Arrange
            int playerId = 1;
            var expectedPlayer = new Player { Id = playerId, Name = "John" };
            _mockPlayerRepository.Setup(repo => repo.GetByIdAsync(playerId))
                .ReturnsAsync(expectedPlayer);

            // Act
            var result = await _playerService.GetPlayerByIdAsync(playerId);

            // Assert
            Assert.Equal(expectedPlayer, result);
        }

        [Fact]
        public async Task GetAllPlayersAsync_ShouldReturnAllPlayers()
        {
            // Arrange
            var expectedPlayers = new List<Player>
            {
                new Player { Id = 1, Name = "John" },
                new Player { Id = 2, Name = "Mary" }
            };
            _mockPlayerRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(expectedPlayers);

            // Act
            var result = await _playerService.GetAllPlayersAsync();

            // Assert
            Assert.Equal(expectedPlayers, result);
        }

        [Fact]
        public async Task CreatePlayerAsync_ShouldCreateNewPlayer()
        {
            // Arrange
            string playerName = "John";
            var expectedPlayer = new Player { Id = 1, Name = playerName };
            _mockPlayerRepository.Setup(repo => repo.CreateAsync(It.IsAny<Player>()))
                .ReturnsAsync(expectedPlayer);

            // Act
            var result = await _playerService.CreatePlayerAsync(playerName);

            // Assert
            Assert.Equal(expectedPlayer, result);
        }

        [Fact]
        public async Task UpdatePlayerAsync_ShouldUpdateExistingPlayer()
        {
            // Arrange
            var existingPlayer = new Player { Id = 1, Name = "John" };
            _mockPlayerRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Player>()))
                .ReturnsAsync(existingPlayer);

            // Act
            var result = await _playerService.UpdatePlayerAsync(existingPlayer);

            // Assert
            Assert.Equal(existingPlayer, result);
        }

        [Fact]
        public async Task DeletePlayerAsync_ShouldDeleteExistingPlayer()
        {
            // Arrange
            int playerId = 1;
            _mockPlayerRepository.Setup(repo => repo.DeleteAsync(playerId))
                .Returns(Task.CompletedTask);

            // Act
            await _playerService.DeletePlayerAsync(playerId);

            // Assert
            _mockPlayerRepository.Verify(repo => repo.DeleteAsync(playerId), Times.Once);
        }
    }
}
