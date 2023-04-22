using PlayBoardGames.Games;

namespace PlayBoardGamesTest.Games
{
	[TestClass]
	public class GameTest
	{
		private const int VALID_USER_ID = 777;
		private readonly Human _human = new("tester", VALID_USER_ID);


		[TestMethod]
		public void ShouldNotBeEndedWhenCreated()
		{
			var game = BuildGame();
			Assert.AreEqual(false, game.IsOver);
		}


		[TestMethod]
		public void ShouldLetValidUserToMakeValidMove()
		{
			var game = BuildGame();
			Assert.IsTrue(game.TryMove(VALID_USER_ID, DummyMove.Play));
			Assert.IsTrue(game.TryMove(VALID_USER_ID, DummyMove.FinishGame));
		}


		[TestMethod]
		public void DoesntLetToMakeValidMoveWhenUserIdIsWrong()
		{
			const int INVALID_UID = VALID_USER_ID + 1;
			var game = BuildGame();

			Assert.ThrowsException<ArgumentException>(() => {
				game.TryMove(INVALID_UID, DummyMove.Play);
			});
		}


		[TestMethod]
		public void ShouldNotLetValidUserToMakeWrongMove()
		{
			var game = BuildGame();
			Assert.IsFalse(game.TryMove(VALID_USER_ID, DummyMove.WrongMove));
		}


		[TestMethod]
		public void ShouldTrulyProxyIsGameOverFlagFromABoard()
		{
			var game = BuildGame();
			game.TryMove(VALID_USER_ID, DummyMove.FinishGame);
			Assert.IsTrue(game.IsOver);
		}


		[TestMethod]
		public void ShouldThrowAnErrorWhenYouTryToMoveAfterFinishingGame()
		{
			var game = BuildGame();
			game.TryMove(VALID_USER_ID, DummyMove.FinishGame);
			Assert.ThrowsException<Exception>(() => game.TryMove(VALID_USER_ID, DummyMove.Play));
		}


		private DummyGame BuildGame()
		{
			return new(new IPlayer[] { _human });
		}
	}


	internal class DummyGame : Game<DummyState, DummyMove>
	{
		public DummyGame(IPlayer[] players) : base(players)
		{ }


		protected override IBoard<DummyState, DummyMove> MakeBoard()
		{
			return new DummyBoard();
		}

		protected override int GetCurrentPlayerIndex(DummyState state)
		{
			return 0;
		}

		protected override bool IsCurrentPlayer(DummyState state, int playerIndex)
		{
			return true;
		}
	}
}
