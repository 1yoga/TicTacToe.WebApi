using TicTacToe.WebApi.Models.Enums;

namespace TicTacToe.WebApi.Models
{
    public class Game
    {
        public int Id { get; set; }
        public int FirstPlayerId { get; set; }
        public Player FirstPlayer { get; set; } // первый игрок
        public int SecondPlayerId { get; set; }
        public Player SecondPlayer { get; set; } // второй игрок
        public string Board { get; set; }
        public int? WinnerId { get; set; } // id игрока, который выиграл
        public bool IsDraw { get; set; } // признак ничьи
        public Status Status { get; set; }
        public virtual ICollection<Move>? Moves { get; set; }
    }
}
