namespace Fluxera.Extensions.Hosting
{
	using System.Threading.Tasks;
	using JetBrains.Annotations;

	/// <summary>
	///     A contract for an application host.
	/// </summary>
	[PublicAPI]
	public interface IApplicationHost
	{
		/// <summary>
		///     Runs the host using the given command line arguments.
		/// </summary>
		/// <param name="args">The command line arguments.</param>
		/// <returns></returns>
		Task RunAsync(string[] args);
	}
}
