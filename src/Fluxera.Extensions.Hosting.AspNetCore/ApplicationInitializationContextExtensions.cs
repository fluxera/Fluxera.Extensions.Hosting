namespace Fluxera.Extensions.Hosting
{
	using JetBrains.Annotations;
	using Microsoft.AspNetCore.Builder;

	[PublicAPI]
	public static class ApplicationInitializationContextExtensions
	{
		public static WebApplication GetApplicationBuilder(this IApplicationInitializationContext context)
		{
			WebApplicationInitializationContext webContext = (WebApplicationInitializationContext) context;
			return (WebApplication)webContext.ApplicationBuilder;
		}

		//public static ApplicationInitializationContext UseDeveloperExceptionPage(
		//	this ApplicationInitializationContext context)
		//{
		//	IApplicationBuilder app = context.GetApplicationBuilder();
		//	ILogger logger = context.CreateLogger<WebModule>();
		//	IHostEnvironment environment = context.GetHostEnvironment();

		//	if (environment.IsDevelopment())
		//	{
		//		context.Log(logger, x => app.UseDeveloperExceptionPage());
		//	}

		//	return context;
		//}

		//public static ApplicationInitializationContext UseExceptionHandler(
		//	this ApplicationInitializationContext context, string errorHandlingPath)
		//{
		//	IApplicationBuilder app = context.GetApplicationBuilder();
		//	ILogger logger = context.CreateLogger<WebModule>();

		//	context.Log(logger, x => app.UseExceptionHandler(errorHandlingPath));

		//	return context;
		//}

		//public static ApplicationInitializationContext UseHsts(this ApplicationInitializationContext context)
		//{
		//	IApplicationBuilder app = context.GetApplicationBuilder();
		//	ILogger logger = context.CreateLogger<WebModule>();

		//	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
		//	// https://docs.microsoft.com/en-us/aspnet/core/migration/20_21?view=aspnetcore-2.1
		//	context.Log(logger, x => app.UseHsts());

		//	return context;
		//}

		//public static ApplicationInitializationContext UseForwardedHeaders(
		//	this ApplicationInitializationContext context)
		//{
		//	IApplicationBuilder app = context.GetApplicationBuilder();
		//	ILogger logger = context.CreateLogger<WebModule>();

		//	context.Log(logger, x => app.UseForwardedHeaders());

		//	return context;
		//}

		//public static ApplicationInitializationContext UseHttpsRedirection(
		//	this ApplicationInitializationContext context)
		//{
		//	IApplicationBuilder app = context.GetApplicationBuilder();
		//	ILogger logger = context.CreateLogger<WebModule>();

		//	// https://docs.microsoft.com/en-us/aspnet/core/migration/20_21?view=aspnetcore-2.1
		//	context.Log(logger, x => app.UseHttpsRedirection());

		//	return context;
		//}

		//public static ApplicationInitializationContext UseStaticFiles(this ApplicationInitializationContext context)
		//{
		//	IApplicationBuilder app = context.GetApplicationBuilder();
		//	ILogger logger = context.CreateLogger<WebModule>();

		//	// https://docs.microsoft.com/en-us/aspnet/core/migration/20_21?view=aspnetcore-2.1
		//	context.Log(logger, x => app.UseStaticFiles());

		//	return context;
		//}
	}
}
