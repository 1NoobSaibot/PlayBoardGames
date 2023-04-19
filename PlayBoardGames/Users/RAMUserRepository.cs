namespace PlayBoardGames.Users
{
	public class RAMUserRepository : IUserRepository
	{
		private readonly Dictionary<int, User> _usersById = new();
		private readonly Dictionary<string, User> _usersByEmail = new();

		public RAMUserRepository()
		{
			AddUserSync(new User() { Id = 1, Email = "user@test.te", Password = "123" });
		}


		public async Task AddUser(User user)
		{
			_usersById.Add(user.Id, user);
			_usersByEmail.Add(user.Email!, user);
		}

		private void AddUserSync(User user)
		{
			_usersById.Add(user.Id, user);
			_usersByEmail.Add(user.Email!, user);
		}


		public async Task<User> GetUserById(int id)
		{
			var user = _usersById[id];
			if (user == null)
			{
				throw new Exception("Cannot find user by ID=" + id);
			}
			return user!;
		}

		public async Task<bool> IsValidUser(string? email, string? password)
		{
			if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
			{
				return false;
			}

			var user = _usersByEmail[email];
			if (user == null || user.Password != password)
			{
				return false;
			}

			return true;
		}
	}
}
