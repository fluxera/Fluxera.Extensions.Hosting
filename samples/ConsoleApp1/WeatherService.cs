namespace ConsoleApp1
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Microsoft.Extensions.Options;

	internal sealed class WeatherService : IWeatherService
	{
		private readonly IOptions<WeatherSettings> weatherSettings;

		public WeatherService(IOptions<WeatherSettings> weatherSettings)
		{
			this.weatherSettings = weatherSettings;
		}

		public Task<IReadOnlyList<int>> GetFiveDayTemperaturesAsync()
		{
			int[] temperatures = { 76, 76, 77, 79, 78 };
			if(this.weatherSettings.Value.Unit.Equals("C", StringComparison.OrdinalIgnoreCase))
			{
				for(int i = 0; i < temperatures.Length; i++)
				{
					temperatures[i] = (int)Math.Round((temperatures[i] - 32) / 1.8);
				}
			}

			return Task.FromResult<IReadOnlyList<int>>(temperatures);
		}
	}
}
