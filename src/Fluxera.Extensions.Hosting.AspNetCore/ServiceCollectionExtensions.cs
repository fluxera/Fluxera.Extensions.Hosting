namespace Fluxera.Extensions.Hosting
{
	using Fluxera.Guards;
	using JetBrains.Annotations;
	using Microsoft.AspNetCore.Mvc.Infrastructure;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.DependencyInjection.Extensions;

	/// <summary>
	///     Extension methods for the <see cref="IServiceCollection" /> type.
	/// </summary>
	[PublicAPI]
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		///     Adds a default implementation for the <see cref="IActionContextAccessor" /> service.
		/// </summary>
		/// <param name="services">The <see cref="IServiceCollection" />.</param>
		/// <returns>The service collection.</returns>
		public static IServiceCollection AddActionContextAccessor(this IServiceCollection services)
		{
			Guard.Against.Null(services, nameof(services));

			services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
			return services;
		}
	}
}
