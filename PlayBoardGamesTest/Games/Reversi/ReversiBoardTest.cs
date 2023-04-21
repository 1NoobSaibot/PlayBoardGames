

using PlayBoardGames.Games.Reversi;

namespace PlayBoardGamesTest.Games.Reversi
{
	[TestClass]
	public class ReversiBoardTest
	{
		[TestMethod]
		public void ShouldConstructNewBoardAccordinglyToRules()
		{
			ReversiBoard board = new();
			Assert.AreEqual(ReversiCell.Black, board.CurrentSide);
			Assert.AreEqual(ReversiCell.White, board[3, 3]);	// d4
			Assert.AreEqual(ReversiCell.White, board[4, 4]);	// e5
			Assert.AreEqual(ReversiCell.Black, board[3, 4]);	// d5
			Assert.AreEqual(ReversiCell.Black, board[4, 3]);	// e4
		}


		[TestMethod]
		public void ShouldTestOpportunitiesToMoveAccordinglyToRules()
		{
			(BoardCreator creator, (int x, int y)[] validMoves)[] samples =
			{
				(new(), new[]{ (2, 3), (3, 2), (4, 5), (5, 4) })
			};

			for (int i = 0; i < samples.Length; i++)
			{
				(var creator, var validMoves) = samples[i];
				
				ForEachMove((x, y) =>
				{
					ReversiBoard board = creator.Create();
					var move = new ReversiMove(x, y);
					bool actual = board.CanMove(move);
					bool expected = validMoves.Contains((x, y));
					Assert.AreEqual(expected, actual);
					Assert.AreEqual(expected, board.TryMove(move));
				});
			}
		}


		private static void ForEachMove(Action<int, int> test)
		{
			for (int x = 0; x < 8; x++)
			{
				for (int y = 0; y < 8; y++)
				{
					test(x, y);
				}
			}
		}
	}


	file class BoardCreator
	{
		private ReversiMove[] _initialMoves;


		public BoardCreator()
		{
			_initialMoves = Array.Empty<ReversiMove>();
		}


		public BoardCreator(params (int x, int y)[] initialMoves)
		{
			_initialMoves = new ReversiMove[initialMoves.Length];
			ReversiBoard testBoard = new ReversiBoard();

			for (int i = 0; i < initialMoves.Length; i++)
			{
				(int x, int y) = initialMoves[i];
				ReversiMove move = new(x, y);

				if (testBoard.TryMove(move) == false)
				{
					throw new ArgumentException($"Cannot make move ({x}, {y}) at index {i}");
				}
				_initialMoves[i] = move;
			}
		}


		public ReversiBoard Create()
		{
			ReversiBoard board = new();

			foreach (var move in _initialMoves)
			{
				if (board.TryMove(move) == false)
				{
					throw new Exception("You should not see the exception");
				}
			}

			return board;
		}
	}
}
