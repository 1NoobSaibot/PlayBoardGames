namespace PlayBoardGames.Games
{
	public interface IGame<TState, TMove>
	{
		bool IsOver { get; }
		TState GetData();
		bool TryMove(int userId, TMove move);
	}
}
