namespace Fluxera.Extensions.Hosting.UnitTests
{
	using System;
	using JetBrains.Annotations;
	using Microsoft.Extensions.DependencyInjection;

	[PublicAPI]
	public abstract class TestBaseWithServiceProvider
	{
		protected abstract IServiceProvider ServiceProvider { get; }

		protected virtual T? GetService<T>()
		{
			return this.ServiceProvider.GetService<T>();
		}

		protected virtual T GetRequiredService<T>() where T : notnull
		{
			return this.ServiceProvider.GetRequiredService<T>();
		}
	}
}
