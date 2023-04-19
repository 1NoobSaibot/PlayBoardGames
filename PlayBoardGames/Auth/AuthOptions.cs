using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PlayBoardGames.Auth
{
	public class AuthOptions
	{
		public const string ISSUER = "PlayBoardGamesUA";
		// public const string AUDIENCE = "MyAuthClient";
		private const string KEY = "mysupersecret_secretkey!123";
		public static SymmetricSecurityKey GetSymmetricSecurityKey() => new(Encoding.UTF8.GetBytes(KEY));

		public static void SetupAuthentication(IServiceCollection services)
		{
			var signinKey = GetSymmetricSecurityKey();

			services
				.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters.IssuerSigningKey = signinKey;
					options.TokenValidationParameters.ValidateIssuerSigningKey = true;
					// options.TokenValidationParameters.TokenDecryptionKey = signinKey;
					options.TokenValidationParameters.ValidAlgorithms = new string[1] { SecurityAlgorithms.HmacSha256 };

					options.TokenValidationParameters.ValidIssuer = ISSUER;
					options.TokenValidationParameters.ValidateIssuer = true;
					

					options.TokenValidationParameters.ValidateActor = false;
					options.TokenValidationParameters.ValidateAudience = false;
					
					options.TokenValidationParameters.ValidateLifetime = true;
					
					options.TokenValidationParameters.ValidateTokenReplay = false;
					
				});
		}


		private static readonly JwtSecurityTokenHandler _jwtHandler = new JwtSecurityTokenHandler();
		public static string GenerateToken(List<Claim> claims)
		{
			var jwt = new JwtSecurityToken(
				issuer: ISSUER,
				// audience: AuthOptions.AUDIENCE,
				claims: claims,
				expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(600)),
				signingCredentials: new SigningCredentials(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
			);

			return _jwtHandler.WriteToken(jwt);
		}
	}
}
