namespace ConsoleApp1
{
	using System;
	using System.Threading.Tasks;
	using Fluxera.Extensions.Hosting;

	public static class Program
	{
		public static async Task Main(string[] args)
		{
			await ApplicationHost.RunAsync<ConsoleApp1Host>(args);

			Console.WriteLine();
			Console.WriteLine("Press any key to quit...");
			Console.ReadKey(true);
		}
	}
}
