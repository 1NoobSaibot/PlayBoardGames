using PlayBoardGames.Games.Reversi;


namespace PlayBoardGamesTest.Games.Reversi
{
	[TestClass]
	public class ReversiStateTest
	{
		[TestMethod]
		public void ShouldTwrowAnExceptionWhenInputArrayIsNot8x8()
		{
			const ReversiCell c = ReversiCell.Empty;
			ReversiState? s;
			Assert.ThrowsException<ArgumentException>(() => s = new ReversiState(new ReversiCell[7, 8], c));
			Assert.ThrowsException<ArgumentException>(() => s = new ReversiState(new ReversiCell[9, 8], c));
			Assert.ThrowsException<ArgumentException>(() => s = new ReversiState(new ReversiCell[8, 7], c));
			Assert.ThrowsException<ArgumentException>(() => s = new ReversiState(new ReversiCell[8, 9], c));
			Assert.ThrowsException<ArgumentException>(() => s = new ReversiState(new ReversiCell[16, 4], c));
		}


		[TestMethod]
		public void Equality()
		{
			ReversiCell[,] cells = new ReversiCell[8, 8];
			ReversiState emptyState1 = new(cells, ReversiCell.Empty);
			ReversiState emptyState2 = new(cells, ReversiCell.Empty);

			Assert.IsTrue(emptyState1 == emptyState2);
			Assert.IsFalse(emptyState1 != emptyState2);
			Assert.IsTrue(emptyState1.Equals(emptyState2));
			Assert.AreEqual(emptyState1, emptyState2);

			ReversiState nonEmptyCurrentSideState = new(cells, ReversiCell.White);
			Assert.IsTrue(nonEmptyCurrentSideState != emptyState1);
			Assert.IsFalse(nonEmptyCurrentSideState == emptyState1);
			Assert.IsFalse(nonEmptyCurrentSideState.Equals(emptyState1));
			Assert.IsFalse(emptyState1.Equals(nonEmptyCurrentSideState));
			Assert.AreNotEqual(emptyState1, nonEmptyCurrentSideState);
			Assert.AreEqual(nonEmptyCurrentSideState, nonEmptyCurrentSideState);

			cells[0, 0] = ReversiCell.Black;
			ReversiState nonEmptyFieldState = new ReversiState(cells, ReversiCell.Empty);
			Assert.IsTrue(nonEmptyFieldState != emptyState1);
			Assert.IsFalse(nonEmptyFieldState == emptyState1);
			Assert.IsFalse(nonEmptyFieldState.Equals(emptyState1));
			Assert.IsFalse(emptyState1.Equals(nonEmptyFieldState));
			Assert.AreNotEqual(emptyState1 , nonEmptyFieldState);
			Assert.AreEqual(nonEmptyFieldState, nonEmptyFieldState);
		}


		[TestMethod]
		public void ShouldCopyArrayDeeply()
		{
			var array = new ReversiCell[8, 8];
			ReversiState state = new(array, ReversiCell.Empty);

			array[0, 0] = ReversiCell.Black;
			Assert.AreNotEqual(array[0, 0], state[0, 0]);
		}
	}
}
