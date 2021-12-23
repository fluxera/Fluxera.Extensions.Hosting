namespace WebApplication1
{
	using Fluxera.Extensions.Hosting;
	using Fluxera.Extensions.Hosting.Modules;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;

	[DependsOn(typeof(WebModule))]
	public class WebApplication1Module : ConfigureApplicationModule
	{
		/// <inheritdoc />
		public override void ConfigureServices(IServiceConfigurationContext context)
		{
			context.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			context.Services.AddEndpointsApiExplorer();
			context.Services.AddSwaggerGen();
		}

		/// <inheritdoc />
		public override void Configure(IApplicationInitializationContext context)
		{
			WebApplication app = context.GetApplicationBuilder();

			// Configure the HTTP request pipeline.
			if(context.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}
