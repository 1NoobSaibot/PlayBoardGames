﻿using PlayBoardGames.Games;

namespace PlayBoardGamesTest.Games
{
	[TestClass]
	public class BoardTest
	{
		[TestMethod]
		public void ShouldNotBeEndedWhenCreated()
		{
			var board = new DummyBoard();
			Assert.AreEqual(false, board.IsGameOver);
		}


		[TestMethod]
		public void ShouldLetToMakeValidMove()
		{
			var board = new DummyBoard();
			Assert.AreEqual(true, board.TryMove(DummyMove.Play));
			Assert.AreEqual(true, board.TryMove(DummyMove.FinishGame));
		}


		[TestMethod]
		public void ShouldNotLetToMakeWrongMove()
		{
			var board = new DummyBoard();
			Assert.AreEqual(false, board.TryMove(DummyMove.WrongMove));
		}


		[TestMethod]
		public void ShouldChangeIsGameOverFlagWhenPlayerFinishGame()
		{
			var board = new DummyBoard();
			board.TryMove(DummyMove.FinishGame);
			Assert.AreEqual(true, board.IsGameOver);
		}


		[TestMethod]
		public void ShouldNotChangeIsGameOverFlagWhenPlayerDoesntFinishGame()
		{
			var board = new DummyBoard();
			board.TryMove(DummyMove.Play);
			Assert.AreEqual(false, board.IsGameOver);
		}


		[TestMethod]
		public void ShouldThrowAnErrorWhenYouTryToMoveAfterFinishingGame()
		{
			var board = new DummyBoard();
			board.TryMove(DummyMove.FinishGame);
			Assert.ThrowsException<Exception>(() => board.TryMove(DummyMove.Play));
		}
	}


	file class DummyBoard : Board<DummyState, DummyMove>
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


	file enum DummyMove
	{
		WrongMove,
		Play,
		FinishGame
	}


	file enum DummyState
	{
		Playing,
		Victory
	}
}
