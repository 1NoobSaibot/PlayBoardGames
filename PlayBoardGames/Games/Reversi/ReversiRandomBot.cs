namespace PlayBoardGames.Games.Reversi
{
	public class ReversiRandomBot : Bot<ReversiState, ReversiMove>
	{
		private readonly Random _rnd = new();


		public ReversiRandomBot() : base(0)
		{ }


		public override ReversiMove GetMove(ReversiState state)
		{
			var moves = CollectMoves(state);
			if (moves.Count() == 0)
			{
				throw new Exception("Cannot find any move");
			}

			int rndIndex = _rnd.Next(moves.Count());
			return moves.ElementAt(rndIndex);
		}


		private IEnumerable<ReversiMove> CollectMoves(IReversiState board)
		{
			List<ReversiMove> res = new();

			for (int x = 0; x < 8; x++)
			{
				for (int y = 0; y < 8; y++)
				{
					var move = new ReversiMove(x, y);
					if (ReversiRules.CanMove(board, move))
					{
						res.Add(move);
					}
				}
			}

			return res;
		}
	}
}
