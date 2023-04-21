namespace PlayBoardGames.Games.Reversi
{
	public interface IReversiState : IReversiBoardArray
	{
		ReversiCell CurrentSide { get; }
	}


	public interface IReversiBoardArray
	{
		ReversiCell this[int x, int y] { get; }
	}


	public enum ReversiCell
	{
		Empty = 0,
		White = 1,
		Black = 2
	}
}
