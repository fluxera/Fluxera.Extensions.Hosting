namespace WebApplication1
{
	using System.Reflection;
	using Fluxera.Extensions.Hosting;
	using Fluxera.Extensions.Hosting.Modules;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.Extensions.DependencyInjection;

	public sealed class WebApplication1Module : ConfigureApplicationModule
	{
		/// <inheritdoc />
		public override void ConfigureServices(IServiceConfigurationContext context)
		{
			context.Services.AddControllers();
			context.Services
				.AddMvc()
				.AddApplicationPart(Assembly.GetExecutingAssembly())
				.AddControllersAsServices();
			context.Services.AddHttpClient();
		}

		/// <inheritdoc />
		public override void Configure(IApplicationInitializationContext context)
		{
			IApplicationBuilder app = context.GetApplicationBuilder();

			app.UseRouting();
			app.UseEndpoints(builder =>
			{
				builder.MapControllers();
			});
		}
	}
}
