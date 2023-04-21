namespace PlayBoardGames.Games.Reversi
{
	public class ReversiState
	{
		private readonly ReversiCell[,] _cells;
		public readonly ReversiCell CurrentSide;

		public ReversiState(ReversiCell[,] cells, ReversiCell currentSide)
		{
			if (cells.GetLength(0) != 8 || cells.GetLength(1) != 8)
			{
				throw new ArgumentException($"Expected array 8x8. Got {cells.GetLength(0)}X{cells.GetLength(1)}");
			}

			_cells = new ReversiCell[8, 8];
			Array.Copy(cells, _cells, 64);
			CurrentSide = currentSide;
		}


		public static bool operator ==(ReversiState lhs, ReversiState rhs)
		{
			if (lhs.CurrentSide != rhs.CurrentSide)
			{
				return false;
			}

			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					if (lhs._cells[i, j] != rhs._cells[i, j])
					{
						return false;
					}
				}
			}

			return true;
		}


		public static bool operator !=(ReversiState lhs, ReversiState rhs)
		{
			return !(lhs == rhs);
		}


		public override bool Equals(object? obj)
		{
			if (obj is ReversiState state)
			{
				return this == state;
			}
			return false;
		}


		public override int GetHashCode()
		{
			return CurrentSide.GetHashCode() ^ _cells.GetHashCode();
		}


		public ReversiCell this[int x, int y] => _cells[x, y];
	}


	public enum ReversiCell
	{
		Empty = 0,
		White = 1,
		Black = 2
	}
}
