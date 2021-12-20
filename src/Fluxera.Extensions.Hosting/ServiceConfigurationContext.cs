namespace Fluxera.Extensions.Hosting
{
	using System.Collections.Generic;
	using Fluxera.Extensions.DependencyInjection;
	using Fluxera.Guards;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using Microsoft.Extensions.Logging;

	[PublicAPI]
	internal sealed class ServiceConfigurationContext : IServiceConfigurationContext
	{
		public ServiceConfigurationContext(IServiceCollection services)
		{
			Guard.Against.Null(services, nameof(services));

			this.Services = services;
			this.Configuration = services.GetObject<IConfiguration>();
			this.Environment = services.GetObject<IHostEnvironment>();
			this.Logger = services.GetObject<ILogger>();

			this.Items = new Dictionary<string, object>();
		}

		public IDictionary<string, object> Items { get; }

		public IServiceCollection Services { get; }

		public IConfiguration Configuration { get; }

		public IHostEnvironment Environment { get; }

		public ILogger Logger { get; }

		/// <summary>
		///     Gets/sets arbitrary named objects those can be stored during
		///     the service registration phase and shared between modules.
		///     This is a shortcut usage of the <see cref="Items" /> dictionary.
		///     Returns null if given key is not found in the <see cref="Items" /> dictionary.
		/// </summary>
		/// <param name="key"></param>
		/// <returns>The stored object or <c>null</c>.</returns>
		public object? this[string key]
		{
			get => this.Items.TryGetValue(key, out object obj) ? obj : null;
			set => this.Items[key] = value;
		}
	}
}
