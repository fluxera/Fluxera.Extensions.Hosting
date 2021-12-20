namespace Fluxera.Extensions.Hosting
{
	using System.Threading.Tasks;
	using JetBrains.Annotations;

	[PublicAPI]
	public interface IApplicationHost
	{
		Task RunAsync(string[] args);
	}
}
