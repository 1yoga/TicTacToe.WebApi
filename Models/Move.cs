using TicTacToe.WebApi.Models.Enums;

namespace TicTacToe.WebApi.Models
{
    public class Move
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public int PlayerId { get; set; }
        public Symbol Symbol { get; set; }
        public int Cell { get; set; } // номер ячейки, куда был сделан ход
    }
}
