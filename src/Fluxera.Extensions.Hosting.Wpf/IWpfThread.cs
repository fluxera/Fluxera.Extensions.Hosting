namespace Fluxera.Extensions.Hosting
{
	using System;

	public interface IWpfThread
	{
		void Initialize(IServiceProvider serviceProvider);

		void Start();

		void Run();
	}
}
