namespace PlayBoardGames.Games.Reversi
{
	public class ReversiBoard : Board<ReversiState, ReversiMove>
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
			return CanSideMove(args, CurrentSide);
		}


		public override ReversiState GetState()
		{
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


		private static ReversiCell Invert(ReversiCell cell)
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


		private void _checkWhoIsNext ()
		{
			var oppositeSide = Invert(CurrentSide);
			if (this.IsThereMoveForSide(oppositeSide))
			{
				CurrentSide = oppositeSide;
			}
			else if (!this.IsThereMoveForSide(CurrentSide))
			{
				CurrentSide = ReversiCell.Empty;
			}
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


		private void SetCell(ReversiMove position)
		{
			_cells[position.X, position.Y] = CurrentSide;

			for (int i = 0; i < DIRECTIONS.Length; i++)
			{
				if (IsLineValid(position, DIRECTIONS[i], CurrentSide))
				{
					InvertLine(position, DIRECTIONS[i]);
				}
			}
		}


		private bool IsThereMoveForSide(ReversiCell side)
		{
			for (int x = 0; x < 8; x++)
			{
				for (int y = 0; y < 8; y++)
				{
					var position = new ReversiMove(x, y);
					if (CanSideMove(position, side))
					{
						return true;
					}
				}
			}

			return false;
		}


		private bool CanSideMove(ReversiMove position, ReversiCell side)
		{
			int x = position.X;
			int y = position.Y;
			if (_cells[x, y] != ReversiCell.Empty)
			{
				return false;
			}

			for (int i = 0; i < DIRECTIONS.Length; i++)
			{
				if (IsLineValid(position, DIRECTIONS[i], side))
				{
					return true;
				}
			}

			return false;
		}


		private bool IsLineValid(ReversiMove position, ReversiMove direction, ReversiCell side)
		{
			var oppositeSide = Invert(side);

			bool isEnemyBetweenUs = false;
			position = new(position.X + direction.X, position.Y + direction.Y);
			while (
				IsPointInsideBoard(position)
				&& this.GetCell(position) != ReversiCell.Empty
			)
			{
				if (this.GetCell(position) == side)
				{
					return isEnemyBetweenUs;
				}
				if (GetCell(position) == oppositeSide)
				{
					isEnemyBetweenUs = true;
				}
				position = new(position.X + direction.X, position.Y + direction.Y);
			}

			return false;
		}


		private void InvertLine(ReversiMove position, ReversiMove direction)
		{
			ReversiMove cell = new(position.X + direction.X, position.Y + direction.Y);
			do
			{
				_cells[cell.X, cell.Y] = Invert(_cells[cell.X, cell.Y]);
				cell = new(cell.X + direction.X, cell.Y + direction.Y);
			} while (_cells[cell.X, cell.Y] != CurrentSide);
		}


		private bool IsPointInsideBoard(ReversiMove point)
		{
			return
				point.X >= 0
				&& point.Y >= 0
				&& point.X <= 7
				&& point.Y <= 7;
		}
	}
}
