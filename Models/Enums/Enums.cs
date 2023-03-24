namespace TicTacToe.WebApi.Models.Enums
{
    public enum Result
    {
        Continue,
        Player1IsWin,
        Player2IsWin,
        IsDraw
    }

    public enum Status
    {
        GameOver,
        NextTurnFirstPlayer,
        NextTurnSecondPlayer,
    }
    
    public enum Symbol
    {
        O,
        X
    }

}
