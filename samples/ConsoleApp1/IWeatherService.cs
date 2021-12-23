namespace ConsoleApp1
{
	using System.Collections.Generic;
	using System.Threading.Tasks;

	internal interface IWeatherService
	{
		Task<IReadOnlyList<int>> GetFiveDayTemperaturesAsync();
	}
}
