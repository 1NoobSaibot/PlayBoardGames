using PlayBoardGames.Users;

namespace PlayBoardGames
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			
			SetupServices(builder.Services);

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			// app.UseHttpsRedirection();
			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllers();
			app.Run();
		}

		protected static void SetupServices(IServiceCollection services)
		{
			services.AddSingleton<IUserRepository>(new RAMUserRepository());

			Auth.AuthOptions.SetupAuthentication(services);

			services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();
		}
	}
}