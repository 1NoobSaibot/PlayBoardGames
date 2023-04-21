using PlayBoardGames.Games;
using PlayBoardGames.Games.Reversi;

namespace PlayBoardGamesTest.Games.Reversi
{
	[TestClass]
	public class ReversiRandomBotTest
	{
		private readonly ReversiRandomBot bot = new();


		[TestMethod]
		public void ShouldAlwaysMakeCorrectMove()
		{
			ReversiBoard board = new();

			do
			{
				ReversiMove move = bot.GetMove(board.GetState());
				Assert.IsTrue(board.TryMove(move));
			} while (board.IsGameOver == false);
		}


		[TestMethod]
		public void ShouldThrowAnExceptionWhenNoCorrectMovesExsists()
		{
			ReversiState noMovesState = new ReversiState(new ReversiCell[8, 8], ReversiCell.Black);
			Assert.ThrowsException<Exception>(() => bot.GetMove(noMovesState));
		}


		[TestMethod]
		public void ShouldMakePlayerDtoAsBotWithZeroComplexity()
		{
			PlayerDto dto = bot.ToPlayerDto();
			Assert.IsTrue(dto.IsBot);
			Assert.AreEqual(0, dto.Complexity);
		}
	}
}
