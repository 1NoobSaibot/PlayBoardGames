using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayBoardGames.Users;
using System.Security.Claims;

namespace PlayBoardGames.Auth
{
	[Route("api/auth")]
	public class AuthController : Controller
	{
		public const string WRONG_CREDENTIALS_MESSAGE = "Incorrect login or password";
		private readonly IUserRepository _users;


		public AuthController(IUserRepository users)
		{
			_users = users;
		}


		[HttpGet("check")]
		[Authorize]
		public async Task<ActionResult> Check()
		{
			return Ok("You are authorized");
		}


		[HttpPost]
		public async Task<ActionResult> Login([FromBody]UserLoginDto credentials)
		{
			bool isUserValid = await _users.IsValidUser(credentials.Login, credentials.Password);
			if (!isUserValid)
			{
				return BadRequest(WRONG_CREDENTIALS_MESSAGE);
			}

			var claims = new List<Claim>() {
				new Claim("login", credentials.Login!),
			};
			string token = AuthOptions.GenerateToken(claims);
			return Content(token);
		}
	}


	public class UserLoginDto
	{
		public string? Login { get; set; }
		public string? Password { get; set; }
	}

}
