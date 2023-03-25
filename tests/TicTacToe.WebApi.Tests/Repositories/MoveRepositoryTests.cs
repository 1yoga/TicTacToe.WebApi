using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.WebApi.Data;
using TicTacToe.WebApi.Models;
using TicTacToe.WebApi.Models.Enums;
using TicTacToe.WebApi.Repositories;

namespace TicTacToe.WebApi.Tests.Repositories
{
    public class MoveRepositoryTests
    {
        private readonly DbContextOptions<TicTacToeContext> _options;
        private readonly TicTacToeContext _dbContext;

        public MoveRepositoryTests()
        {
            // Initialize a new database for each test method
            _options = new DbContextOptionsBuilder<TicTacToeContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new TicTacToeContext(_options);
        }

        public void Dispose()
        {
            // Dispose the database after each test method
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenMoveDoesNotExist()
        {
            // Arrange
            var repository = new MoveRepository(_dbContext);

            // Act
            var move = await repository.GetByIdAsync(1);

            // Assert
            Assert.Null(move);
        }

        [Fact]
        public async Task GetAllByGameIdAsync_ReturnsEmptyList_WhenNoMovesExistForGame()
        {
            // Arrange
            var repository = new MoveRepository(_dbContext);

            // Act
            var moves = await repository.GetAllByGameIdAsync(1);

            // Assert
            Assert.Empty(moves);
        }

        [Fact]
        public async Task CreateAsync_CreatesNewMove()
        {
            // Arrange
            var repository = new MoveRepository(_dbContext);

            var move = new Move
            {
                GameId = 1,
                PlayerId = 1,
                Symbol = Symbol.X,
                Cell = 1
            };

            // Act
            var createdMove = await repository.CreateAsync(move);

            // Assert
            Assert.NotNull(createdMove);
            Assert.Equal(move.GameId, createdMove.GameId);
            Assert.Equal(move.PlayerId, createdMove.PlayerId);
            Assert.Equal(move.Symbol, createdMove.Symbol);
            Assert.Equal(move.Cell, createdMove.Cell);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesMove_WhenValidMoveIsProvided()
        {
            // Arrange
            var repository = new MoveRepository(_dbContext);

            var move = new Move
            {
                GameId = 1,
                PlayerId = 1,
                Symbol = Symbol.X,
                Cell = 1
            };

            await repository.CreateAsync(move);

            move.Symbol = Symbol.O;
            move.Cell = 2;

            // Act
            await repository.UpdateAsync(move);

            var updatedMove = await repository.GetByIdAsync(move.Id);

            // Assert
            Assert.NotNull(updatedMove);
            Assert.Equal(move.GameId, updatedMove.GameId);
            Assert.Equal(move.PlayerId, updatedMove.PlayerId);
            Assert.Equal(move.Symbol, updatedMove.Symbol);
            Assert.Equal(move.Cell, updatedMove.Cell);
        }

        [Fact]
        public async Task DeleteAsync_DeletesMove_WhenValidMoveIdIsProvided()
        {
            // Arrange
            var repository = new MoveRepository(_dbContext);

            var move = new Move
            {
                GameId = 1,
                PlayerId = 1,
                Symbol = Symbol.X,
                Cell = 1
            };

            await repository.CreateAsync(move);

            // Act
            await repository.DeleteAsync(move.Id);

            var deletedMove = await repository.GetByIdAsync(move.Id);

            // Assert
            Assert.Null(deletedMove);
        }

    }
}
