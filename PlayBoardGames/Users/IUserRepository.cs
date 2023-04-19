namespace PlayBoardGames.Users
{
	public interface IUserRepository
	{
		Task AddUser(User user);
		Task<User> GetUserById(int id);
		Task<bool> IsValidUser(string? login, string? password);
	}
}
