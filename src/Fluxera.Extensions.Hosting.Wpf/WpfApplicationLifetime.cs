namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Threading;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using Microsoft.Extensions.Logging;
	using Microsoft.Extensions.Options;

	internal sealed class WpfApplicationLifetime : IHostLifetime, IDisposable
	{
		private readonly IHostApplicationLifetime applicationLifetime;
		private readonly IHostEnvironment hostEnvironment;
		private readonly ILogger logger;

		private readonly ManualResetEvent manualResetEvent = new ManualResetEvent(false);
		private readonly WpfApplicationLifetimeOptions options;
		private readonly IServiceProvider serviceProvider;
		private readonly WpfContext wpfContext;

		private CancellationTokenRegistration applicationStartedRegistration;
		private CancellationTokenRegistration applicationStoppingRegistration;
		private Thread? thread;

		public WpfApplicationLifetime(
			IServiceProvider serviceProvider,
			WpfContext wpfContext,
			IHostApplicationLifetime applicationLifetime,
			IHostEnvironment hostEnvironment,
			IOptions<WpfApplicationLifetimeOptions> options,
			ILoggerFactory loggerFactory)
		{
			this.serviceProvider = serviceProvider;
			this.wpfContext = wpfContext;
			this.applicationLifetime = applicationLifetime;
			this.hostEnvironment = hostEnvironment;
			this.logger = loggerFactory.CreateLogger("Microsoft.Hosting.Lifetime");
			this.options = options.Value;
		}

		/// <inheritdoc />
		public void Dispose()
		{
			this.manualResetEvent.Set();
			this.manualResetEvent.Dispose();

			this.applicationStartedRegistration.Dispose();
			this.applicationStoppingRegistration.Dispose();
		}

		public async Task WaitForStartAsync(CancellationToken cancellationToken)
		{
			if(!this.options.SuppressStatusMessages)
			{
				this.applicationStartedRegistration = this.applicationLifetime.ApplicationStarted.Register(() =>
				{
					this.logger.LogInformation("Application started.");
					this.logger.LogInformation("Hosting environment: {EnvironmentName}", this.hostEnvironment.EnvironmentName);
					this.logger.LogInformation("Content root path: {ContentRootPath}", this.hostEnvironment.ContentRootPath);
				});

				this.applicationStoppingRegistration = this.applicationLifetime.ApplicationStopping.Register(() =>
				{
					this.logger.LogInformation("Application is shutting down...");
				});
			}

			// Wait for the ui thread to be started.
			await this.StartApplicationThreadAsync(cancellationToken);
		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{
			try
			{
				bool checkAccess = this.wpfContext.Dispatcher.CheckAccess();
				if(checkAccess)
				{
					this.wpfContext.Application.Shutdown();
				}
				else
				{
					await this.wpfContext.Dispatcher.InvokeAsync(() => this.wpfContext.Application.Shutdown());
				}
			}
			catch(OperationCanceledException)
			{
				// Intentionally left blank.
			}
		}

		private async Task StartApplicationThreadAsync(CancellationToken cancellationToken)
		{
			TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();

			// Create and start the WPF thread.
			// ReSharper disable once UseObjectOrCollectionInitializer
			this.thread = new Thread(_ =>
			{
				// Prepare the application.
				this.PreApplicationStart(tcs);

				// Wait for the application startup.
				this.manualResetEvent.WaitOne();

				// Run the action application.
				this.RunApplication(tcs);
			});

			// Set the thread as background thread.
			this.thread.IsBackground = true;

			// Set the apartment state.
			this.thread.SetApartmentState(ApartmentState.STA);

			// Start the new UI thread.
			this.thread.Start(this.serviceProvider);

			// Make the UI thread go.
			this.manualResetEvent.Set();

			// Try to cancel if cancellation is requested.
			cancellationToken.ThrowIfCancellationRequested();

			// Wait for the thread to start.
			await tcs.Task;
		}

		private void PreApplicationStart(TaskCompletionSource<object> taskCompletionSource)
		{
			// Create the application instance.
			Application application = new Application
			{
				// Set the shutdown mode of the application.
				ShutdownMode = this.wpfContext.ShutdownMode,
			};

			// Store the application for others to interact.
			this.wpfContext.Application = application;

			// Create our SynchronizationContext, and install it.
			SynchronizationContext.SetSynchronizationContext(
				new DispatcherSynchronizationContext(Dispatcher.CurrentDispatcher));

			// Register to the WPF application exit to stop the host application.
			application.Exit += (_, _) =>
			{
				IHostApplicationLifetime hostApplicationLifetime = this.serviceProvider.GetRequiredService<IHostApplicationLifetime>();
				hostApplicationLifetime.StopApplication();
			};

			application.Startup += (_, _) =>
			{
				// Open the specified shell window.
				IMainWindow? mainWindow = this.serviceProvider.GetService<IMainWindow>();
				if(mainWindow != null)
				{
					taskCompletionSource.SetResult(true);
					mainWindow.Show();
				}
				else
				{
					const string message = "The main window was not available. The application is now terminated.";

					this.logger.LogCritical(message);

					MessageBox.Show(
						$"The main window was not available.{Environment.NewLine}{Environment.NewLine}The application is now terminated.",
						"Fatal error",
						MessageBoxButton.OK,
						MessageBoxImage.Error,
						MessageBoxResult.OK);

					taskCompletionSource.SetException(new InvalidOperationException(message));
				}
			};
		}

		private void RunApplication(TaskCompletionSource<object> taskCompletionSource)
		{
			try
			{
				// Use the provided application initializer.
				IEnumerable<IWpfApplicationInitializer> wpfServices = this.serviceProvider.GetServices<IWpfApplicationInitializer>();
				foreach(IWpfApplicationInitializer wpfService in wpfServices)
				{
					wpfService.Initialize(this.wpfContext.Application);
				}

				// Run the WPF application in this thread which was specifically created for it.
				this.wpfContext.Application.Run();
			}
			catch(Exception ex)
			{
				taskCompletionSource.SetException(ex);
			}
		}
	}
}
