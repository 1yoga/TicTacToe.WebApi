using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.WebApi.Controllers;
using TicTacToe.WebApi.Models;
using TicTacToe.WebApi.Services;

namespace TicTacToe.WebApi.Tests.Controllers
{
    public class PlayerControllerTests
    {
        private readonly Mock<IPlayerService> _playerServiceMock;
        private readonly PlayerController _controller;

        public PlayerControllerTests()
        {
            _playerServiceMock = new Mock<IPlayerService>();
            _controller = new PlayerController(_playerServiceMock.Object);
        }

        [Fact]
        public async Task GetPlayerById_ReturnsOkObjectResult_WhenPlayerExists()
        {
            // Arrange
            int playerId = 1;
            var expectedPlayer = new Player { Id = playerId, Name = "John" };
            _playerServiceMock.Setup(s => s.GetPlayerByIdAsync(playerId)).ReturnsAsync(expectedPlayer);

            // Act
            var result = await _controller.GetPlayerById(playerId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualPlayer = Assert.IsType<Player>(okResult.Value);
            Assert.Equal(expectedPlayer.Id, actualPlayer.Id);
            Assert.Equal(expectedPlayer.Name, actualPlayer.Name);
        }

        [Fact]
        public async Task GetPlayerById_ReturnsNotFoundResult_WhenPlayerDoesNotExist()
        {
            // Arrange
            int playerId = 1;
            _playerServiceMock.Setup(s => s.GetPlayerByIdAsync(playerId)).ReturnsAsync(null as Player);

            // Act
            var result = await _controller.GetPlayerById(playerId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreatePlayer_ReturnsOkObjectResult_WhenPlayerCreated()
        {
            // Arrange
            string playerName = "John";
            var expectedPlayer = new Player { Name = playerName };
            _playerServiceMock.Setup(s => s.CreatePlayerAsync(playerName)).ReturnsAsync(expectedPlayer);

            // Act
            var result = await _controller.CreatePlayer(playerName);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualPlayer = Assert.IsType<Player>(okResult.Value);
            Assert.Equal(expectedPlayer.Id, actualPlayer.Id);
            Assert.Equal(expectedPlayer.Name, actualPlayer.Name);
        }

        [Fact]
        public async Task UpdatePlayer_ReturnsOkObjectResult_WhenPlayerUpdated()
        {
            // Arrange
            int playerId = 1;
            var playerToUpdate = new Player { Id = playerId, Name = "John" };
            _playerServiceMock.Setup(s => s.UpdatePlayerAsync(playerToUpdate)).ReturnsAsync(playerToUpdate);

            // Act
            var result = await _controller.UpdatePlayer(playerId, playerToUpdate);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualPlayer = Assert.IsType<Player>(okResult.Value);
            Assert.Equal(playerToUpdate.Id, actualPlayer.Id);
            Assert.Equal(playerToUpdate.Name, actualPlayer.Name);
        }

        [Fact]
        public async Task UpdatePlayer_ReturnsBadRequestResult_WhenPlayerIdDoesNotMatch()
        {
            // Arrange
            int playerId = 1;
            var playerToUpdate = new Player { Id = playerId + 1, Name = "John" };

            // Act
            var result = await _controller.UpdatePlayer(playerId, playerToUpdate);

            // Assert
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public async Task DeletePlayer_ReturnsNoContentResult_WhenPlayerDeleted()
        {
            // Arrange
            var playerId = 1;
            var playerServiceMock = new Mock<IPlayerService>();
            playerServiceMock.Setup(x => x.GetPlayerByIdAsync(playerId)).ReturnsAsync(new Player { Id = playerId });
            var controller = new PlayerController(playerServiceMock.Object);

            // Act
            var result = await controller.DeletePlayer(playerId);

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
            playerServiceMock.Verify(x => x.DeletePlayerAsync(playerId), Times.Once);
        }
    }
}
