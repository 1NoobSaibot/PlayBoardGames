using PlayBoardGames.Games;

namespace PlayBoardGamesTest.Games
{
	internal class DummyBoard : Board<DummyState, DummyMove>
	{
		private DummyMove? _lastMove;

		public override bool CanMove(DummyMove moveArgs)
		{
			return moveArgs != DummyMove.WrongMove;
		}

		public override DummyState GetState()
		{
			if (_lastMove == null || _lastMove == DummyMove.Play)
			{
				return DummyState.Playing;
			}
			return DummyState.Victory;
		}

		protected override bool CheckIsGameOver()
		{
			return _lastMove == DummyMove.FinishGame;
		}

		protected override void Move(DummyMove moveArgs)
		{
			_lastMove = moveArgs;
		}
	}


	internal enum DummyMove
	{
		WrongMove,
		Play,
		FinishGame
	}


	internal enum DummyState
	{
		Playing,
		Victory
	}
}
