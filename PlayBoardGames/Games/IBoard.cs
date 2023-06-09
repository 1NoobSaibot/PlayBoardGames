﻿namespace PlayBoardGames.Games
{
	public interface IBoard<TState, TMove>
	{
		bool CanMove(TMove move);
		TState GetState();
		bool IsGameOver { get; }
		bool Move(TMove move);
	}
}
