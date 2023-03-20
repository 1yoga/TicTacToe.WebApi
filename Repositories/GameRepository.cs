using Microsoft.EntityFrameworkCore;
using System;
using TicTacToe.WebApi.Data;
using TicTacToe.WebApi.Models;

namespace TicTacToe.WebApi.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly TicTacToeContext _dbContext;

        public GameRepository(TicTacToeContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Game>> GetAllAsync()
        {
            return await _dbContext.Set<Game>().ToListAsync();
        }

        public async Task<Game> GetByIdAsync(int id)
        {
            return await _dbContext.Set<Game>().FindAsync(id);
        }

        public async Task<Game> CreateAsync(Game game)
        {
            await _dbContext.Set<Game>().AddAsync(game);
            await _dbContext.SaveChangesAsync();
            return game;
        }

        public async Task<Game> UpdateAsync(Game game)
        {
            _dbContext.Entry(game).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return game;
        }

        public async Task DeleteAsync(Game game)
        {
            _dbContext.Set<Game>().Remove(game);
            await _dbContext.SaveChangesAsync();
        }
    }
}
