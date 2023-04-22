namespace PlayBoardGames.Games
{
	public abstract class Game<TState, TMove> : IGame<TState, TMove>
	{
		private readonly IBoard<TState, TMove> _board;
		private readonly List<(TState state, TMove move)> _history = new();
		private readonly IPlayer[] _players;
		public bool IsOver => _board.IsGameOver;


		public Game(IPlayer[] players)
		{
			_board = MakeBoard();
			_players = players;
		}


		protected abstract IBoard<TState, TMove> MakeBoard();
		protected abstract bool IsCurrentPlayer(TState state, int playerIndex);
		protected abstract int GetCurrentPlayerIndex(TState state);


		public TState GetData()
		{
			return _board.GetState();
		}


		public bool TryMove(int userId, TMove move)
		{
			int playerIndex = GetPlayerIndexByUserId(userId);
			var prevState = _board.GetState();

			if (IsCurrentPlayer(prevState, playerIndex) == false)
			{
				return false;
			}

			if (_board.TryMove(move) == false)
			{
				return false;
			}

			_history.Add((prevState, move));

			while (!IsOver && GetCurrentPlayer() is Bot<TState, TMove> bot)
			{
				prevState = _board.GetState();
				move = bot.GetMove(prevState);
				_board.TryMove(move);
				_history.Add((prevState, move));
			}

			return true;
		}


		private IPlayer GetCurrentPlayer()
		{
			return _players[GetCurrentPlayerIndex(_board.GetState())];
		}



		private int GetPlayerIndexByUserId(int userId)
		{
			for (int i = 0; i < _players.Length; i++)
			{
				if (_players[i] is Human human && human.UserId == userId)
				{
					return i;
				}
			}

			throw new ArgumentException($"Cannot find a user (id={userId})");
		}
	}
}
