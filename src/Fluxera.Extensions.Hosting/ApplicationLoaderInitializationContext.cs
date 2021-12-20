namespace Fluxera.Extensions.Hosting
{
	using System;

	public class ApplicationLoaderInitializationContext : IApplicationLoaderInitializationContext
	{
		public ApplicationLoaderInitializationContext(IServiceProvider serviceProvider)
		{
			this.ServiceProvider = serviceProvider;
		}

		public IServiceProvider ServiceProvider { get; }
	}
}
