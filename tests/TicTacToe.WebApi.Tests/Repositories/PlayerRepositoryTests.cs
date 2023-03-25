using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.WebApi.Data;
using TicTacToe.WebApi.Models;
using TicTacToe.WebApi.Repositories;

namespace TicTacToe.WebApi.Tests.Repositories
{
    public class PlayerRepositoryTests
    {
        private readonly DbContextOptions<TicTacToeContext> _options;
        private readonly PlayerRepository _repository;

        public PlayerRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<TicTacToeContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _repository = new PlayerRepository(new TicTacToeContext(_options));
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsPlayerFromDatabase()
        {
            // Arrange
            var player = new Player { Name = "TestPlayer" };
            using (var context = new TicTacToeContext(_options))
            {
                await context.Players.AddAsync(player);
                await context.SaveChangesAsync();
            }

            // Act
            var result = await _repository.GetByIdAsync(player.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(player.Id, result.Id);
            Assert.Equal(player.Name, result.Name);
        }

        [Fact]
        public async Task GetByNameAsync_ReturnsPlayerFromDatabase()
        {
            // Arrange
            var player = new Player { Name = "TestPlayer" };
            using (var context = new TicTacToeContext(_options))
            {
                await context.Players.AddAsync(player);
                await context.SaveChangesAsync();
            }

            // Act
            var result = await _repository.GetByNameAsync(player.Name);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(player.Id, result.Id);
            Assert.Equal(player.Name, result.Name);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsListOfPlayersFromDatabase()
        {
            // Arrange
            var players = new List<Player>
        {
            new Player { Name = "TestPlayer1" },
            new Player { Name = "TestPlayer2" }
        };
            using (var context = new TicTacToeContext(_options))
            {
                await context.Players.AddRangeAsync(players);
                await context.SaveChangesAsync();
            }

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(players.Count, result.Count);
            Assert.Equal(players[0].Id, result[0].Id);
            Assert.Equal(players[0].Name, result[0].Name);
            Assert.Equal(players[1].Id, result[1].Id);
            Assert.Equal(players[1].Name, result[1].Name);
        }

        [Fact]
        public async Task CreateAsync_CreatesNewPlayerInDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TicTacToeContext>()
                .UseInMemoryDatabase(databaseName: "CreateAsync_CreatesNewPlayerInDatabase")
                .Options;
            using var dbContext = new TicTacToeContext(options);
            var playerRepository = new PlayerRepository(dbContext);

            var playerName = "John Doe";

            // Act
            var createdPlayer = await playerRepository.CreateAsync(new Player { Name = playerName });

            // Assert
            Assert.NotNull(createdPlayer);
            Assert.NotEqual(default(int), createdPlayer.Id);
            Assert.Equal(playerName, createdPlayer.Name);

            var playerInDatabase = await playerRepository.GetByIdAsync(createdPlayer.Id);
            Assert.Equal(createdPlayer, playerInDatabase);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesPlayerInDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TicTacToeContext>()
                .UseInMemoryDatabase(databaseName: "UpdateAsync_UpdatesPlayerInDatabase")
                .Options;
            using (var context = new TicTacToeContext(options))
            {
                var player = new Player { Name = "Alice" };
                await context.Players.AddAsync(player);
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new TicTacToeContext(options))
            {
                var playerRepository = new PlayerRepository(context);
                var playerToUpdate = await playerRepository.GetByNameAsync("Alice");
                playerToUpdate.Name = "Bob";
                await playerRepository.UpdateAsync(playerToUpdate);
            }

            // Assert
            using (var context = new TicTacToeContext(options))
            {
                var player = await context.Players.SingleAsync();
                Assert.Equal("Bob", player.Name);
            }
        }

        [Fact]
        public async Task DeleteAsync_DeletesPlayerFromDatabase()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TicTacToeContext>()
                .UseInMemoryDatabase(databaseName: "DeleteAsync_DeletesPlayerFromDatabase")
                .Options;
            using (var context = new TicTacToeContext(options))
            {
                var player = new Player { Name = "Alice" };
                await context.Players.AddAsync(player);
                await context.SaveChangesAsync();
            }

            // Act
            using (var context = new TicTacToeContext(options))
            {
                var playerRepository = new PlayerRepository(context);
                var playerToDelete = await playerRepository.GetByNameAsync("Alice");
                await playerRepository.DeleteAsync(playerToDelete.Id);
            }

            // Assert
            using (var context = new TicTacToeContext(options))
            {
                var players = await context.Players.ToListAsync();
                Assert.Empty(players);
            }
        }

    }
}
