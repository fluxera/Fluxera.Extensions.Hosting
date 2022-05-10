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

		/// <inheritdoc />
		public IDictionary<string, object> Items { get; }

		/// <inheritdoc />
		public IServiceCollection Services { get; }

		/// <inheritdoc />
		public IConfiguration Configuration { get; }

		/// <inheritdoc />
		public IHostEnvironment Environment { get; }

		/// <inheritdoc />
		public ILogger Logger { get; }

		/// <inheritdoc />
		public object this[string key]
		{
			get => this.Items.TryGetValue(key, out object obj) ? obj : null;
			set => this.Items[key] = value!;
		}

		/// <inheritdoc />
		IServiceCollection ILoggingContext<IServiceCollection>.LogContextData => this.Services;
	}
}
