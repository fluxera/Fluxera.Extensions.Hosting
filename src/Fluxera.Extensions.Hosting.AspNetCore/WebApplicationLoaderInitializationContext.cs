namespace Fluxera.Extensions.Hosting
{
	using Microsoft.AspNetCore.Builder;

	internal sealed class WebApplicationLoaderInitializationContext : ApplicationLoaderInitializationContext
	{
		public WebApplicationLoaderInitializationContext(IApplicationBuilder applicationBuilder)
			: base(applicationBuilder.ApplicationServices)
		{
			this.ApplicationBuilder = applicationBuilder;
		}

		public IApplicationBuilder ApplicationBuilder { get; }
	}
}
