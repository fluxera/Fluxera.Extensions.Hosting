namespace Fluxera.Extensions.Hosting
{
	using Fluxera.Guards;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Hosting;

	/// <summary>
	///     Extension methods for the <see cref="IHostEnvironment" /> type.
	/// </summary>
	[PublicAPI]
	public static class HostEnvironmentExtensions
	{
		private static readonly string Testing = "Testing";

		/// <summary>
		///     Checks if the current host environment name is <see cref="Testing" />.
		/// </summary>
		/// <param name="hostEnvironment">An instance of <see cref="IHostEnvironment" />.</param>
		/// <returns>True if the environment name is <see cref="Testing" />, otherwise false.</returns>
		public static bool IsTesting(this IHostEnvironment hostEnvironment)
		{
			hostEnvironment = Guard.Against.Null(hostEnvironment);

			return hostEnvironment.IsEnvironment(Testing);
		}
	}
}
