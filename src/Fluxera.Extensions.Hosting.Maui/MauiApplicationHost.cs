namespace Fluxera.Extensions.Hosting
{
	using Fluxera.Extensions.Hosting.Modules;
	using JetBrains.Annotations;

	/// <summary>
	///     An abstract base class for MAUI host applications.
	/// </summary>
	/// <typeparam name="TStartupModule"></typeparam>
	[PublicAPI]
	public abstract class MauiApplicationHost<TStartupModule> : ApplicationHost<TStartupModule>
		where TStartupModule : class, IModule
	{
	}
}
