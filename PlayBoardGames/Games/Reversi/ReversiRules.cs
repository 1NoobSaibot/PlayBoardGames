namespace PlayBoardGames.Games.Reversi
{
	public static class ReversiRules
	{
		public static ReversiCell InvertCell(ReversiCell cell)
		{
			if (cell == ReversiCell.White)
			{
				return ReversiCell.Black;
			}

			if (cell == ReversiCell.Black)
			{
				return ReversiCell.White;
			}

			throw new Exception("Inverting an Empty-Cell detected. It is probably logic error");
		}


		public static bool IsItPossibleToMove(IReversiState state)
		{
			return IsItPossibleToMoveFor(state, state.CurrentSide);
		}


		public static bool IsItPossibleToMoveFor(IReversiBoardArray board, ReversiCell side)
		{
			for (int x = 0; x < 8; x++)
			{
				for (int y = 0; y < 8; y++)
				{
					var position = new ReversiMove(x, y);
					if (CanMove(board, position, side))
					{
						return true;
					}
				}
			}

			return false;
		}


		private static readonly ReversiMove[] DIRECTIONS = {
			new( 0,  1),
			new( 1,  1),
			new( 1,  0),
			new( 1, -1),
			new( 0, -1),
			new(-1, -1),
			new(-1,  0),
			new(-1,  1)
		};


		public static bool CanMove(IReversiState board, ReversiMove position)
		{
			return CanMove(board, position, board.CurrentSide);
		}


		public static bool CanMove(
			IReversiBoardArray board,
			ReversiMove position,
			ReversiCell currentSide
		)
		{
			int x = position.X;
			int y = position.Y;
			if (board[x, y] != ReversiCell.Empty)
			{
				return false;
			}

			for (int i = 0; i < DIRECTIONS.Length; i++)
			{
				if (IsLineValid(board, position, DIRECTIONS[i], currentSide))
				{
					return true;
				}
			}

			return false;
		}


		public static bool IsLineValid(
			IReversiBoardArray board,
			ReversiMove position,
			ReversiMove direction,
			ReversiCell currentSide
		)
		{
			var oppositeSide = InvertCell(currentSide);

			bool isEnemyBetweenUs = false;
			position = new(position.X + direction.X, position.Y + direction.Y);
			while (
				IsPointInsideBoard(position)
				&& board[position.X, position.Y] != ReversiCell.Empty
			)
			{
				if (board[position.X, position.Y] == currentSide)
				{
					return isEnemyBetweenUs;
				}
				if (board[position.X, position.Y] == oppositeSide)
				{
					isEnemyBetweenUs = true;
				}
				position = new(position.X + direction.X, position.Y + direction.Y);
			}

			return false;
		}


		private static bool IsPointInsideBoard(ReversiMove point)
		{
			return
				point.X >= 0
				&& point.Y >= 0
				&& point.X <= 7
				&& point.Y <= 7;
		}
	}
}
