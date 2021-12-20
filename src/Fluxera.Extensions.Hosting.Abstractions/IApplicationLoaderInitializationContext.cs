namespace Fluxera.Extensions.Hosting
{
	using System;
	using JetBrains.Annotations;

	[PublicAPI]
	public interface IApplicationLoaderInitializationContext
	{
		IServiceProvider ServiceProvider { get; }
	}
}
