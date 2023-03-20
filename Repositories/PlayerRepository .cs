using Microsoft.EntityFrameworkCore;
using System;
using TicTacToe.WebApi.Data;
using TicTacToe.WebApi.Models;

namespace TicTacToe.WebApi.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly TicTacToeContext _dbContext;

        public PlayerRepository(TicTacToeContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Player> GetByIdAsync(int id)
        {
            return await _dbContext.Players.FindAsync(id);
        }

        public async Task<Player> GetByNameAsync(string name)
        {
            return await _dbContext.Players.SingleOrDefaultAsync(x => x.Name == name);
        }

        public async Task<List<Player>> GetAllAsync()
        {
            return await _dbContext.Players.ToListAsync();
        }

        public async Task<Player> CreateAsync(Player player)
        {
            await _dbContext.Players.AddAsync(player);
            await _dbContext.SaveChangesAsync();
            return player;
        }

        public async Task<Player> UpdateAsync(Player player)
        {
            _dbContext.Players.Update(player);
            await _dbContext.SaveChangesAsync();
            return player;
        }

        public async Task DeleteAsync(int id)
        {
            var player = await GetByIdAsync(id);
            if (player != null)
            {
                _dbContext.Players.Remove(player);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
