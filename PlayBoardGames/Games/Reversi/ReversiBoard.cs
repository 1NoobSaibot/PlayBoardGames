namespace PlayBoardGames.Games.Reversi
{
	public class ReversiBoard : Board<ReversiState, ReversiMove>, IReversiState
	{
		private readonly ReversiCell[,] _cells = new ReversiCell[8, 8];
		public ReversiCell CurrentSide { get; private set; } = ReversiCell.Black;


		public ReversiBoard()
		{
			_cells[3, 3] = ReversiCell.White;
			_cells[3, 4] = ReversiCell.Black;
			_cells[4, 3] = ReversiCell.Black;
			_cells[4, 4] = ReversiCell.White;
		}


		public ReversiBoard(ReversiCell[,] cells, ReversiCell currentSide)
		{
			if (cells.GetLength(0) != 8 || cells.GetLength(1) != 8)
			{
				string actualSize = cells.GetLength(0) + "x" + cells.GetLength(1);
				throw new ArgumentException($"Array cells must be 8x8. Got {actualSize}");
			}
			_cells = cells;
			CurrentSide = currentSide;
		}


		public bool TryMove(int x, int y)
		{
			return TryMove(new ReversiMove(x, y));
		}


		public override bool CanMove(ReversiMove args)
		{
			return ReversiRules.CanMove(this, args);
		}


		public override ReversiState GetState()
		{
			// Don't return board(this) as a state, because it's mutable
			return new ReversiState(_cells, CurrentSide);
		}


		public ReversiCell GetCell(ReversiMove point)
		{
			return _cells[point.X, point.Y];
		}


		public ReversiCell this[int x, int y] => _cells[x, y];


		protected override void Move(ReversiMove position)
		{
			SetCell(position);
			_checkWhoIsNext();
		}


		protected override bool CheckIsGameOver()
		{
			return CurrentSide == ReversiCell.Empty;
		}


		private void _checkWhoIsNext ()
		{
			CurrentSide = ReversiRules.InvertCell(CurrentSide);
			if (ReversiRules.IsItPossibleToMove(this))
			{
				// If we have a move for next player
				return;
			}

			// We don't have a move for next player
			CurrentSide = ReversiRules.InvertCell(CurrentSide);
			if (ReversiRules.IsItPossibleToMove(this))
			{
				// If we have a move for previous player
				return;
			}

			// We have no moves for Black and White sides. Game is over
			CurrentSide = ReversiCell.Empty;
		}


		private static readonly IReadOnlyList<ReversiMove> DIRECTIONS = new ReversiMove[]{
			new( 0,  1),
			new( 1,  1),
			new( 1,  0),
			new( 1, -1),
			new( 0, -1),
			new(-1, -1),
			new(-1,  0),
			new(-1,  1)
		};


		private void SetCell(ReversiMove position)
		{
			_cells[position.X, position.Y] = CurrentSide;

			for (int i = 0; i < DIRECTIONS.Count; i++)
			{
				if (ReversiRules.IsLineValid(this, position, DIRECTIONS[i], CurrentSide))
				{
					InvertLine(position, DIRECTIONS[i]);
				}
			}
		}


		private void InvertLine(ReversiMove position, ReversiMove direction)
		{
			ReversiMove cell = new(position.X + direction.X, position.Y + direction.Y);
			do
			{
				_cells[cell.X, cell.Y] = ReversiRules.InvertCell(_cells[cell.X, cell.Y]);
				cell = new(cell.X + direction.X, cell.Y + direction.Y);
			} while (_cells[cell.X, cell.Y] != CurrentSide);
		}
	}
}
