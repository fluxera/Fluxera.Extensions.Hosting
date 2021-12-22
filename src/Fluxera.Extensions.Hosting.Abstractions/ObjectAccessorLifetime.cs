namespace Fluxera.Extensions.Hosting
{
	using Fluxera.Extensions.DependencyInjection;
	using JetBrains.Annotations;

	/// <summary>
	///     A custom object accessor context that is used to control the lifetime of an object in ser service collection.
	/// </summary>
	[PublicAPI]
	public sealed class ObjectAccessorLifetime
	{
		/// <summary>
		///     The object accessor will only be available while configuring the services.
		/// </summary>
		public static ObjectAccessorContext ConfigureServices = new ObjectAccessorContext("ConfigureServices");

		/// <summary>
		///     The object accessor will be available while configuring the services and the application.
		/// </summary>
		public static ObjectAccessorContext Configure = new ObjectAccessorContext("Configure");

		/// <summary>
		///     The object accessor will be available at any time over the lifetime of the application.
		/// </summary>
		public static ObjectAccessorContext Application = new ObjectAccessorContext("Application");
	}
}
