namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Hosting;

	/// <summary>
	///     Base class for implementing a long running <see cref="IHostedService" />.
	/// </summary>
	[PublicAPI]
	public abstract class XamarinBackgroundService : IXamarinHostedService, IDisposable
	{
		private Task _executingTask;
		private CancellationTokenSource _stoppingCts;

		public virtual void Dispose()
		{
			this._stoppingCts.Cancel();
		}

		/// <summary>
		///     Triggered when the application host is ready to start the service.
		/// </summary>
		/// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
		public virtual Task StartAsync(CancellationToken cancellationToken)
		{
			this._stoppingCts = new CancellationTokenSource();

			// Store the task we're executing
			this._executingTask = this.ExecuteAsync(this._stoppingCts.Token);

			// If the task is completed then return it, this will bubble cancellation and failure to the caller
			if(this._executingTask.IsCompleted)
			{
				return this._executingTask;
			}

			// Otherwise it's running
			return Task.CompletedTask;
		}

		/// <summary>
		///     Triggered when the application host is performing a graceful shutdown.
		/// </summary>
		/// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
		public virtual async Task StopAsync(CancellationToken cancellationToken)
		{
			// Stop called without start
			if(this._executingTask == null)
			{
				return;
			}

			try
			{
				// Signal cancellation to the executing method
				this._stoppingCts.Cancel();
			}
			finally
			{
				// Wait until the task completes or the stop token triggers
				await Task.WhenAny(this._executingTask, Task.Delay(Timeout.Infinite, cancellationToken));
			}
		}

		/// <summary>
		///     Triggered when the application host is sleeping.
		/// </summary>
		/// <param name="cancellationToken">Indicates that the sleep process should no longer be graceful.</param>
		public async Task SleepAsync(CancellationToken cancellationToken)
		{
			await this.StopAsync(cancellationToken);
		}

		/// <summary>
		///     Triggered when the application host is ready to resume the service.
		/// </summary>
		/// <param name="cancellationToken">Indicates that the resume process has been aborted.</param>
		public Task ResumeAsync(CancellationToken cancellationToken)
		{
			return this.StartAsync(cancellationToken);
		}

		/// <summary>
		///     This method is called when the <see cref="IHostedService" /> starts. The implementation should return a task that
		///     represents
		///     the lifetime of the long running operation(s) being performed.
		/// </summary>
		/// <param name="stoppingToken">Triggered when <see cref="IHostedService.StopAsync(CancellationToken)" /> is called.</param>
		/// <returns>A <see cref="Task" /> that represents the long running operations.</returns>
		protected abstract Task ExecuteAsync(CancellationToken stoppingToken);
	}
}
