using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using PlayBoardGames;
using PlayBoardGames.Auth;
using PlayBoardGames.Users;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace PlayBoardGamesTest.Auth
{
	[TestClass]
	public class AuthenticationTest
	{
		private readonly User user = new() { Id = 1, Email = "user@test.te", Password = "123" };
		private readonly WebApplicationFactory<Program> _factory
			= new WebApplicationFactory<Program>();


		[TestMethod]
		public async Task ShouldAuthorizeValidUser()
		{
			var client = _factory.CreateDefaultClient();
			var req = new HttpRequestMessage(HttpMethod.Post, "api/auth");
			req.Content = JsonContent.Create(new Dictionary<string, string> {
				{ "login", user.Email! },
				{ "password", user.Password! }
			});
			var res = await client.SendAsync(req);

			Assert.AreEqual(HttpStatusCode.OK, res.StatusCode);
			string token = await res.Content.ReadAsStringAsync();
			AssertIsJwtToken(token);
		}


		[TestMethod]
		public async Task ShouldNOTAuthorizeUserWithWrongCredentials()
		{
			var client = _factory.CreateDefaultClient();
			var req = new HttpRequestMessage(HttpMethod.Post, "api/auth");
			req.Content = JsonContent.Create(new Dictionary<string, string> {
				{ "login", user.Email! },
				{ "password", user.Password! + "misstake" }
			});
			var res = await client.SendAsync(req);

			Assert.AreEqual(HttpStatusCode.BadRequest, res.StatusCode);
			string content = await res.Content.ReadAsStringAsync();
			Assert.AreEqual(AuthController.WRONG_CREDENTIALS_MESSAGE, content);
		}


		[TestMethod]
		public async Task ShouldNotLetDefaultUserToGetProtectedResource()
		{
			var httpClient = _factory.CreateDefaultClient();

			var response = await httpClient.GetAsync("api/auth/check");
			Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
			var strRes = await response.Content.ReadAsStringAsync();

			Assert.AreEqual(string.Empty, strRes);
		}


		[TestMethod]
		public async Task ShouldLetAuthorizedUserToGetProtectedResource()
		{
			var httpClient = _factory.CreateDefaultClient();
			HttpRequestMessage loginReq = new(HttpMethod.Post, "api/auth");
			loginReq.Content = JsonContent.Create(new Dictionary<string, string> {
				{ "login", user.Email! },
				{ "password", user.Password! }
			});
			var resWithToken = await httpClient.SendAsync(loginReq);
			var token = await resWithToken.Content.ReadAsStringAsync();

			httpClient.DefaultRequestHeaders.Authorization
				= new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, token);
			var response = await httpClient.GetAsync("api/auth/check");

			Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
			var strRes = await response.Content.ReadAsStringAsync();
			Assert.AreEqual("You are authorized", strRes);
		}


		private static void AssertIsJwtToken(string? token)
		{
			Assert.AreNotEqual(null, token);
			NUnit.Framework.Assert
				.That(token, Does.Match("^[\\w-]+\\.[\\w-]+\\.[\\w-]+$"));
		}
	}
}
