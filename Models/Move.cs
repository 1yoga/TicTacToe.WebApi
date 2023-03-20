namespace TicTacToe.WebApi.Models
{
    public class Move
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; } // игровое поле, к которому относится ход
        public int PlayerId { get; set; }
        public Player Player { get; set; } // игрок, который сделал ход
        public int Cell { get; set; } // номер ячейки, куда был сделан ход
    }
}
