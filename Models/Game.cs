namespace TicTacToe.WebApi.Models
{
    public class Game
    {
        public int Id { get; set; }
        public int Player1Id { get; set; }
        public Player Player1 { get; set; } // первый игрок
        public int Player2Id { get; set; }
        public Player Player2 { get; set; } // второй игрок
        public string Board { get; set; }
        public int? WinnerId { get; set; } // id игрока, который выиграл
        public bool IsDraw { get; set; } // признак ничьи
    }
}
