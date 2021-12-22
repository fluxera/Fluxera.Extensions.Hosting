namespace Fluxera.Extensions.Hosting
{
	using System;

	/// <summary>
	///     The default implementation.
	/// </summary>
	public class ApplicationLoaderInitializationContext : IApplicationLoaderInitializationContext
	{
		/// <summary>
		///     Creates a new instance of the <see cref="ApplicationLoaderInitializationContext" /> type.
		/// </summary>
		/// <param name="serviceProvider"></param>
		public ApplicationLoaderInitializationContext(IServiceProvider serviceProvider)
		{
			this.ServiceProvider = serviceProvider;
		}

		/// <inheritdoc />
		public IServiceProvider ServiceProvider { get; }
	}
}
