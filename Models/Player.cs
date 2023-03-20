namespace TicTacToe.WebApi.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; } // символ, которым игрок играет (X или O)
    }
}
