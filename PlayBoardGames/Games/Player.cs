namespace PlayBoardGames.Games
{
	public interface IPlayer
	{
		PlayerDto ToPlayerDto();
	}


	public abstract class Bot<TState, TMove> : IPlayer
	{
		public readonly int Complexity;


		public Bot(int complexity)
		{
			Complexity = complexity;
		}


		public abstract TMove GetMove(TState state);


		public PlayerDto ToPlayerDto()
		{
			return PlayerDto.DescribeBot(Complexity);
		}
	}


	public class Human : IPlayer
	{
		public readonly string UserName;
		public readonly int UserId;


		public Human(string userName, int userId)
		{
			UserName = userName;
			UserId = userId;
		}


		public PlayerDto ToPlayerDto()
		{
			return PlayerDto.DescribeHuman(UserName, UserId);
		}
	}


	public class PlayerDto
	{
		public readonly bool IsBot;

		#region BotDescription
		public readonly int? Complexity;
		#endregion

		#region UserDescription
		public readonly string? UserName;
		public readonly int? UserId;
		#endregion


		private PlayerDto(bool isBot, int? complexity, string? userName, int? userId)
		{
			IsBot = isBot;
			Complexity = complexity;
			UserName = userName;
			UserId = userId;
		}


		public static PlayerDto DescribeBot(int Complexity)
		{
			return new PlayerDto(true, Complexity, null, null);
		}


		public static PlayerDto DescribeHuman(string userName, int userId)
		{
			return new PlayerDto(false, null, userName, userId);
		}
	}
}
