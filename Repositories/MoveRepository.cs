using Microsoft.EntityFrameworkCore;
using System;
using TicTacToe.WebApi.Data;
using TicTacToe.WebApi.Models;

namespace TicTacToe.WebApi.Repositories
{
    public class MoveRepository : IMoveRepository
    {
        private readonly TicTacToeContext _dbContext;

        public MoveRepository(TicTacToeContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Move> GetByIdAsync(int id)
        {
            return await _dbContext.Moves.FindAsync(id);
        }

        public async Task<List<Move>> GetAllByGameIdAsync(int gameId)
        {
            return await _dbContext.Moves.Where(m => m.GameId == gameId).ToListAsync();
        }

        public async Task<Move> CreateAsync(Move move)
        {
            await _dbContext.Moves.AddAsync(move);
            await _dbContext.SaveChangesAsync();
            return move;
        }

        public async Task UpdateAsync(Move move)
        {
            _dbContext.Moves.Update(move);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Move move)
        {
            _dbContext.Moves.Remove(move);
            await _dbContext.SaveChangesAsync();
        }
    }
}
