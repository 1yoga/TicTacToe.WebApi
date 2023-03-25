using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.WebApi.Data;
using TicTacToe.WebApi.Models.Enums;
using TicTacToe.WebApi.Models;
using TicTacToe.WebApi.Repositories;

namespace TicTacToe.WebApi.Tests.Repositories
{
    public class GameRepositoryTests
    {
        private DbContextOptions<TicTacToeContext> _options;
        private TicTacToeContext _dbContext;
        private GameRepository _gameRepository;

        public GameRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<TicTacToeContext>()
                .UseInMemoryDatabase(databaseName: "TicTacToeTest")
                .Options;
            _dbContext = new TicTacToeContext(_options);
            _gameRepository = new GameRepository(_dbContext);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllGames()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TicTacToeContext>()
                .UseInMemoryDatabase(databaseName: "GetAllAsync_ShouldReturnAllGames")
                .Options;
            using var dbContext = new TicTacToeContext(options);
            var gameRepository = new GameRepository(dbContext);


            await dbContext.Games.AddRangeAsync(
                new Game { FirstPlayerId = 1, SecondPlayerId = 2, Status = Status.NextTurnFirstPlayer, Board = "         " },
                new Game { FirstPlayerId = 3, SecondPlayerId = 4, Status = Status.NextTurnFirstPlayer, Board = "         " },
                new Game { FirstPlayerId = 2, SecondPlayerId = 1, Status = Status.NextTurnFirstPlayer, Board = "         " }
            );
            await dbContext.SaveChangesAsync();

            // Act
            var games = await gameRepository.GetAllAsync();

            // Assert
            Assert.Equal(3, games.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnGameById()
        {
            // Arrange
            var game = new Game { FirstPlayerId = 1, SecondPlayerId = 2, Status = Status.NextTurnFirstPlayer, Board = "         " };
            await _dbContext.Games.AddAsync(game);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _gameRepository.GetByIdAsync(game.Id);

            // Assert
            Assert.Equal(game.Id, result.Id);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateGame()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TicTacToeContext>()
                .UseInMemoryDatabase(databaseName: "CreateAsync_ShouldCreateGame")
                .Options;
            using var dbContext = new TicTacToeContext(options);
            var gameRepository = new GameRepository(dbContext);

            // Act
            var game = new Game { FirstPlayerId = 1, SecondPlayerId = 2, Status = Status.NextTurnFirstPlayer, Board = "         " };
            var result = await gameRepository.CreateAsync(game);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(game.FirstPlayerId, result.FirstPlayerId);
            Assert.Equal(game.SecondPlayerId, result.SecondPlayerId);
            Assert.Equal(game.Status, result.Status);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateGame()
        {
            // Arrange
            var game = new Game { FirstPlayerId = 1, SecondPlayerId = 2, Status = Status.NextTurnFirstPlayer, Board = "         " };
            await _dbContext.Games.AddAsync(game);
            await _dbContext.SaveChangesAsync();
            game.Status = Status.NextTurnSecondPlayer;

            // Act
            var result = await _gameRepository.UpdateAsync(game);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(game.Id, result.Id);
            Assert.Equal(game.Status, result.Status);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteGame()
        {
            // Arrange
            var game = new Game { FirstPlayerId = 1, SecondPlayerId = 2, Status = Status.NextTurnFirstPlayer, Board = "         " };
            await _dbContext.Games.AddAsync(game);
            await _dbContext.SaveChangesAsync();

            // Act
            await _gameRepository.DeleteAsync(game.Id);

            // Assert
            var result = await _gameRepository.GetByIdAsync(game.Id);
            Assert.Null(result);
        }
    }
}
