namespace PlayBoardGames.Games
{
	public abstract class Board<TState, TMove> : IBoard<TState, TMove>
	{
		private bool _isGameOver = false;
		public bool IsGameOver => _isGameOver;
		public abstract TState GetState();
		public abstract bool CanMove(TMove moveArgs);

		public bool Move(TMove moveArgs) {
			if (_isGameOver) {
				throw new Exception("Moving after the game is over");
			}
			if (!CanMove(moveArgs)) {
				return false;
			}

			_Move(moveArgs);
			if (_CheckIsGameOver()) {
				_setGameOver();
			}
			return true;
		}

		protected abstract void _Move(TMove moveArgs);
		protected abstract bool _CheckIsGameOver();

		private void _setGameOver()
		{
			_isGameOver = true;
		}
	}
}
