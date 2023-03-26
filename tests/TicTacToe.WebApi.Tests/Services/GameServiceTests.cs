using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.WebApi.Models.Enums;
using TicTacToe.WebApi.Models;
using TicTacToe.WebApi.Repositories;
using TicTacToe.WebApi.Services;

namespace TicTacToe.WebApi.Tests.Services
{
    public class GameServiceTests
    {
        private readonly Mock<IGameRepository> _gameRepositoryMock;
        private readonly Mock<IPlayerRepository> _playerRepositoryMock;
        private readonly Mock<IMoveRepository> _moveRepositoryMock;
        private readonly GameService _gameService;

        public GameServiceTests()
        {
            _gameRepositoryMock = new Mock<IGameRepository>();
            _playerRepositoryMock = new Mock<IPlayerRepository>();
            _moveRepositoryMock = new Mock<IMoveRepository>();

            _gameService = new GameService(_gameRepositoryMock.Object, _playerRepositoryMock.Object, _moveRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllGames()
        {
            // Arrange
            var games = new List<Game>
            {
                new Game { Id = 1, Board = "XO XOXOX ", FirstPlayerId = 1, SecondPlayerId = 2, Status = Status.GameOver, WinnerId = 1 },
                new Game { Id = 2, Board = "         ", FirstPlayerId = 3, SecondPlayerId = 4, Status = Status.NextTurnFirstPlayer },
                new Game { Id = 3, Board = "XOXO  X  ", FirstPlayerId = 1, SecondPlayerId = 5, Status = Status.GameOver, WinnerId = 1 }
            };
            _gameRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(games);

            // Act
            var result = await _gameService.GetAllAsync();

            // Assert
            Assert.Equal(games, result);
        }

        [Fact]
        public async Task GetByIdAsync_WithExistingGame_ShouldReturnGame()
        {
            // Arrange
            var game = new Game { Id = 1, Board = "XO XOXOX ", FirstPlayerId = 1, SecondPlayerId = 2, Status = Status.GameOver, WinnerId = 1 };
            _gameRepositoryMock.Setup(x => x.GetByIdAsync(game.Id)).ReturnsAsync(game);

            // Act
            var result = await _gameService.GetByIdAsync(game.Id);

            // Assert
            Assert.Equal(game, result);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateNewGame()
        {
            // Arrange
            var firstPlayerId = 1;
            var secondPlayerId = 2;
            var game = new Game { Id = 1, Board = "         ", FirstPlayerId = firstPlayerId, SecondPlayerId = secondPlayerId, Status = Status.NextTurnFirstPlayer };
            _gameRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Game>())).ReturnsAsync(game);

            // Act
            var result = await _gameService.CreateAsync(firstPlayerId, secondPlayerId);

            // Assert
            _gameRepositoryMock.Verify(x => x.CreateAsync(It.Is<Game>(g => g.FirstPlayerId == firstPlayerId && g.SecondPlayerId == secondPlayerId)), Times.Once);
            Assert.Equal(game, result);
        }

        [Fact]
        public async Task CreateMoveAsync_WithExistingMove_ShouldThrowApplicationException()
        {
            // Arrange
            var gameId = 1;
            var playerId = 2;
            var cell = 5;
            var existingMove = new Move
            {
                GameId = gameId,
                PlayerId = playerId,
                Cell = cell,
                Symbol = Symbol.X
            };
            var existingMoves = new List<Move> { existingMove };
            _moveRepositoryMock.Setup(x => x.GetAllByGameIdAsync(gameId)).ReturnsAsync(existingMoves);

            // Act & Assert
            await Assert.ThrowsAsync<ApplicationException>(() => _gameService.CreateMoveAsync(gameId, playerId, cell));
        }

        [Fact]
        public async Task CreateMoveAsync_WithInvalidGameId_ShouldThrowApplicationException()
        {
            // Arrange
            var gameId = 1;
            _gameRepositoryMock.Setup(x => x.GetByIdAsync(gameId)).ReturnsAsync((Game)null);

            // Act & Assert
            await Assert.ThrowsAsync<ApplicationException>(() => _gameService.CreateMoveAsync(gameId, 1, 5));
        }

        [Fact]
        public async Task CreateMoveAsync_WithWinningMove_ShouldSetWinnerIdAndStatusToGameOver()
        {
            // Arrange
            var gameId = 1;
            var playerId = 2;
            var cell1 = 0;
            var cell2 = 4;
            var cell3 = 1;
            var cell4 = 5;
            var cell5 = 6;
            var existingMoves = new List<Move>
            {
                new Move { GameId = gameId, PlayerId = playerId, Cell = cell1, Symbol = Symbol.X },
                new Move { GameId = gameId, PlayerId = playerId, Cell = cell2, Symbol = Symbol.O },
                new Move { GameId = gameId, PlayerId = playerId, Cell = cell3, Symbol = Symbol.X },
                new Move { GameId = gameId, PlayerId = playerId, Cell = cell4, Symbol = Symbol.O }
            };
            _moveRepositoryMock.Setup(x => x.GetAllByGameIdAsync(gameId)).ReturnsAsync(existingMoves);
            var game = new Game { Board = "X OXO    ", FirstPlayerId = 1, SecondPlayerId = 2, Status = Status.NextTurnSecondPlayer };
            _gameRepositoryMock.Setup(x => x.GetByIdAsync(gameId)).ReturnsAsync(game);

            // Act
            var result = await _gameService.CreateMoveAsync(gameId, playerId, cell5);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(playerId, result.WinnerId);
            Assert.Equal(Status.GameOver, result.Status);
        }

        [Fact]
        public async Task CreateMoveAsync_WithDraw_ShouldSetIsDrawAndStatusToGameOver()
        {
            // Arrange
            var gameRepositoryMock = new Mock<IGameRepository>();
            var playerRepositoryMock = new Mock<IPlayerRepository>();
            var moveRepositoryMock = new Mock<IMoveRepository>();

            var gameService = new GameService(gameRepositoryMock.Object, playerRepositoryMock.Object, moveRepositoryMock.Object);

            var gameId = 1;
            var firstPlayerId = 1;
            var secondPlayerId = 2;
            var cell1 = 0;
            var cell2 = 1;
            var cell3 = 2;
            var cell4 = 3;
            var cell5 = 4;
            var cell6 = 5;
            var cell7 = 6;
            var cell8 = 7;
            var cell9 = 8;

            var game = new Game
            {
                Id = gameId,
                FirstPlayerId = firstPlayerId,
                SecondPlayerId = secondPlayerId,
                Board = "XOXXXOO O",
                Status = Status.NextTurnFirstPlayer,
            };

            var moves = new List<Move>
            {
                new Move { GameId = gameId, PlayerId = firstPlayerId, Cell = cell1, Symbol = Symbol.X },
                new Move { GameId = gameId, PlayerId = secondPlayerId, Cell = cell2, Symbol = Symbol.O },
                new Move { GameId = gameId, PlayerId = firstPlayerId, Cell = cell3, Symbol = Symbol.X },
                new Move { GameId = gameId, PlayerId = secondPlayerId, Cell = cell4, Symbol = Symbol.O },
                new Move { GameId = gameId, PlayerId = firstPlayerId, Cell = cell5, Symbol = Symbol.X },
                new Move { GameId = gameId, PlayerId = secondPlayerId, Cell = cell6, Symbol = Symbol.O },
                new Move { GameId = gameId, PlayerId = firstPlayerId, Cell = cell7, Symbol = Symbol.X },
                new Move { GameId = gameId, PlayerId = firstPlayerId, Cell = cell9, Symbol = Symbol.O },
            };

            game.WinnerId = null;
            game.IsDraw = false;
            game.Moves = moves;

            gameRepositoryMock.Setup(x => x.GetByIdAsync(gameId)).ReturnsAsync(game);
            moveRepositoryMock.Setup(x => x.GetAllByGameIdAsync(gameId)).ReturnsAsync(moves);

            // Act
            var result = await gameService.CreateMoveAsync(gameId, firstPlayerId, cell8);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(gameId, result.Id);
            Assert.Equal(secondPlayerId, result.SecondPlayerId);
            Assert.True(result.IsDraw);
            Assert.Equal(Status.GameOver, result.Status);
        }

    }
}
