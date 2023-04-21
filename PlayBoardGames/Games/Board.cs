namespace PlayBoardGames.Games
{
	public abstract class Board<TState, TMove> : IBoard<TState, TMove>
	{
		public bool IsGameOver { get; private set; } = false;
		public abstract TState GetState();
		public abstract bool CanMove(TMove moveArgs);


		public bool TryMove(TMove moveArgs) {
			if (IsGameOver)
			{
				throw new Exception("Moving after the game is over");
			}
			if (!CanMove(moveArgs))
			{
				return false;
			}

			Move(moveArgs);
			if (CheckIsGameOver())
			{
				SetGameOver();
			}
			return true;
		}


		protected abstract void Move(TMove moveArgs);
		protected abstract bool CheckIsGameOver();


		private void SetGameOver()
		{
			IsGameOver = true;
		}
	}
}
