using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.WebApi.Controllers;
using TicTacToe.WebApi.Models;
using TicTacToe.WebApi.Models.Enums;
using TicTacToe.WebApi.Services;
using Xunit;

namespace TicTacToe.WebApi.Tests.Controllers
{
    public class GameControllerTests
    {
        private readonly Mock<IGameService> _gameServiceMock;
        private readonly Mock<IPlayerService> _playerServiceMock;
        private readonly GameController _controller;

        public GameControllerTests()
        {
            _gameServiceMock = new Mock<IGameService>();
            _playerServiceMock = new Mock<IPlayerService>();
            _controller = new GameController(_gameServiceMock.Object, _playerServiceMock.Object);
        }

        [Fact]
        public async Task GetAllGames_ReturnsOkResult_WhenGamesExist()
        {
            // Arrange
            var games = new List<Game>
            {
                new Game(),
                new Game(),
                new Game()
            };

            _gameServiceMock.Setup(service => service.GetAllAsync()).ReturnsAsync(games);

            // Act
            var result = await _controller.GetAllGames();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualGames = Assert.IsType<Game[]>(okResult.Value);
            Assert.Equal(3, actualGames.Count());
        }

        [Fact]
        public async Task GetGameById_ReturnsNotFound_WhenGameIsNotFound()
        {
            // Arrange
            int gameId = 1;
            _gameServiceMock.Setup(x => x.GetByIdAsync(gameId)).ReturnsAsync((Game)null);

            // Act
            var result = await _controller.GetGameById(gameId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetGameById_ReturnsOk_WhenGameIsFound()
        {
            // Arrange
            int gameId = 1;
            var game = new Game { Id = gameId };
            _gameServiceMock.Setup(x => x.GetByIdAsync(gameId)).ReturnsAsync(game);

            // Act
            var result = await _controller.GetGameById(gameId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsType<Game>(okResult.Value);
            Assert.Equal(gameId, model.Id);
        }

        [Fact]
        public async Task CreateGame_ReturnsNotFound_WhenFirstPlayerIsNotFound()
        {
            // Arrange
            int firstPlayerId = 1;
            int secondPlayerId = 2;
            _playerServiceMock.Setup(x => x.GetPlayerByIdAsync(firstPlayerId)).ReturnsAsync((Player)null);

            // Act
            var result = await _controller.CreateGame(firstPlayerId, secondPlayerId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal($"Player with ID {firstPlayerId} was not found", (result.Result as NotFoundObjectResult).Value);
        }

        [Fact]
        public async Task CreateGame_ReturnsNotFound_WhenSecondPlayerIsNotFound()
        {
            // Arrange
            int firstPlayerId = 1;
            int secondPlayerId = 2;
            var firstPlayer = new Player { Id = firstPlayerId };
            _playerServiceMock.Setup(x => x.GetPlayerByIdAsync(firstPlayerId)).ReturnsAsync(firstPlayer);
            _playerServiceMock.Setup(x => x.GetPlayerByIdAsync(secondPlayerId)).ReturnsAsync((Player)null);

            // Act
            var result = await _controller.CreateGame(firstPlayerId, secondPlayerId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal($"Player with ID {secondPlayerId} was not found", (result.Result as NotFoundObjectResult).Value);
        }

        [Fact]
        public async Task CreateGame_ReturnsBadRequest_WhenFirstPlayerAndSecondPlayerAreTheSame()
        {
            // Arrange
            int firstPlayerId = 1;
            int secondPlayerId = 1;
            var player = new Player { Id = firstPlayerId };
            _playerServiceMock.Setup(x => x.GetPlayerByIdAsync(firstPlayerId)).ReturnsAsync(player);

            // Act
            var result = await _controller.CreateGame(firstPlayerId, secondPlayerId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal($"Players must be different", (result.Result as BadRequestObjectResult).Value);
        }

        [Fact]
        public async Task CreateGame_ReturnsOkResult_WhenPlayersExistAndGameCreatedSuccessfully()
        {
            // Arrange
            var firstPlayerId = 1;
            var secondPlayerId = 2;
            var firstPlayer = new Player { Id = firstPlayerId, Name = "John" };
            var secondPlayer = new Player { Id = secondPlayerId, Name = "Jane" };
            var createdGame = new Game { Id = 1, FirstPlayerId = firstPlayerId, SecondPlayerId = secondPlayerId, Status = Status.NextTurnFirstPlayer };
            _playerServiceMock.Setup(x => x.GetPlayerByIdAsync(firstPlayerId)).ReturnsAsync(firstPlayer);
            _playerServiceMock.Setup(x => x.GetPlayerByIdAsync(secondPlayerId)).ReturnsAsync(secondPlayer);
            _gameServiceMock.Setup(x => x.CreateAsync(firstPlayerId, secondPlayerId)).ReturnsAsync(createdGame);

            // Act
            var result = await _controller.CreateGame(firstPlayerId, secondPlayerId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedGame = Assert.IsType<Game>(okResult.Value);
            Assert.Equal(createdGame, returnedGame);
        }

        [Fact]
        public async Task CreateMove_ReturnsBadRequest_WhenGameIsOver()
        {
            // Arrange
            int gameId = 1;
            int playerId = 2;
            int cell = 3;
            var game = new Game
            {
                Id = gameId,
                Status = Status.GameOver,
                FirstPlayerId = playerId,
                SecondPlayerId = playerId + 1
            };
            var player = new Player { Id = playerId };

            _gameServiceMock.Setup(x => x.GetByIdAsync(gameId)).ReturnsAsync(game);
            _playerServiceMock.Setup(x => x.GetPlayerByIdAsync(playerId)).ReturnsAsync(player);

            // Act
            var result = await _controller.CreateMove(gameId, playerId, cell);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CreateMove_ReturnsBadRequest_WhenNotPlayersTurn()
        {
            // Arrange
            int gameId = 1;
            int playerId = 2;
            int cell = 3;
            var game = new Game
            {
                Id = gameId,
                Status = Status.NextTurnFirstPlayer,
                FirstPlayerId = playerId + 1,
                SecondPlayerId = playerId
            };
            var player = new Player { Id = playerId };

            _gameServiceMock.Setup(x => x.GetByIdAsync(gameId)).ReturnsAsync(game);
            _playerServiceMock.Setup(x => x.GetPlayerByIdAsync(playerId)).ReturnsAsync(player);

            // Act
            var result = await _controller.CreateMove(gameId, playerId, cell);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CreateMove_ReturnsBadRequest_WhenMoveIsNotValid()
        {
            // Arrange
            int gameId = 1;
            int playerId = 2;
            int cell = 3;
            var game = new Game
            {
                Id = gameId,
                Status = Status.NextTurnFirstPlayer,
                FirstPlayerId = playerId,
                SecondPlayerId = playerId + 1
            };
            var player = new Player { Id = playerId };
            string message = "Invalid move";

            _gameServiceMock.Setup(x => x.GetByIdAsync(gameId)).ReturnsAsync(game);
            _playerServiceMock.Setup(x => x.GetPlayerByIdAsync(playerId)).ReturnsAsync(player);
            _gameServiceMock.Setup(x => x.CreateMoveAsync(gameId, playerId, cell)).Throws(new ApplicationException(message));

            // Act
            var result = await _controller.CreateMove(gameId, playerId, cell);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(message, badRequestResult.Value);
        }

        [Fact]
        public async Task CreateMove_ReturnsOk_WhenMoveIsValid()
        {
            // Arrange
            int gameId = 1;
            int playerId = 2;
            int cell = 3;
            var gameServiceMock = new Mock<IGameService>();
            var playerServiceMock = new Mock<IPlayerService>();
            var game = new Game
            {
                Id = gameId,
                FirstPlayerId = 1,
                SecondPlayerId = 2,
                Status = Status.NextTurnSecondPlayer,
                Board = "         "
            };
            gameServiceMock.Setup(x => x.GetByIdAsync(gameId)).ReturnsAsync(game);
            playerServiceMock.Setup(x => x.GetPlayerByIdAsync(playerId)).ReturnsAsync(new Player());

            // Act
            var controller = new GameController(gameServiceMock.Object, playerServiceMock.Object);
            var result = await controller.CreateMove(gameId, playerId, cell);

            // Assert
            gameServiceMock.Verify(x => x.CreateMoveAsync(gameId, playerId, cell), Times.Once);
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
