namespace ConsoleApp1
{
	using System;
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;
	using Microsoft.Extensions.Hosting;
	using Microsoft.Extensions.Logging;

	internal sealed class ConsoleHostedService : IHostedService
	{
		private readonly IHostApplicationLifetime appLifetime;
		private readonly ILogger logger;
		private readonly IWeatherService weatherService;

		private int? exitCode;

		public ConsoleHostedService(
			ILogger<ConsoleHostedService> logger,
			IHostApplicationLifetime appLifetime,
			IWeatherService weatherService)
		{
			this.logger = logger;
			this.appLifetime = appLifetime;
			this.weatherService = weatherService;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			this.logger.LogDebug($"Starting with arguments: {string.Join(" ", Environment.GetCommandLineArgs())}");

			this.appLifetime.ApplicationStarted.Register(() =>
			{
				Task.Run(async () =>
				{
					try
					{
						IReadOnlyList<int> temperatures = await this.weatherService.GetFiveDayTemperaturesAsync();
						for(int i = 0; i < temperatures.Count; i++)
						{
							this.logger.LogInformation($"{DateTime.Today.AddDays(i).DayOfWeek}: {temperatures[i]}");
						}

						this.exitCode = 0;
					}
					catch(Exception ex)
					{
						this.logger.LogError(ex, "Unhandled exception!");
						this.exitCode = 1;
					}
					finally
					{
						// Stop the application once the work is done
						this.appLifetime.StopApplication();
					}
				}, cancellationToken);
			});

			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			this.logger.LogDebug($"Exiting with return code: {this.exitCode}");

			// Exit code may be null if the user cancelled via Ctrl+C/SIGTERM
			Environment.ExitCode = this.exitCode.GetValueOrDefault(-1);
			return Task.CompletedTask;
		}
	}
}
