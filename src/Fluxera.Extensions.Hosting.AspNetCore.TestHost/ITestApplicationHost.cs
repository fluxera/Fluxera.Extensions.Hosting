namespace Fluxera.Extensions.Hosting
{
	using JetBrains.Annotations;
	using Microsoft.AspNetCore.TestHost;

	/// <summary>
	///     A contract for a test host.
	/// </summary>
	[PublicAPI]
	public interface ITestApplicationHost : IApplicationHost
	{
		/// <summary>
		///		Gets the test server instance.
		/// </summary>
		TestServer Server { get; }
	}
}