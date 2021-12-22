namespace Fluxera.Extensions.Hosting
{
	using System;
	using System.Linq.Expressions;
	using Fluxera.Guards;
	using JetBrains.Annotations;
	using Microsoft.Extensions.Logging;

	/// <summary>
	///     Extension methods to support easier logging in application configuration methods of modules.
	/// </summary>
	[PublicAPI]
	public static class ApplicationInitializationContextExtensions
	{
		/// <summary>
		///     Logs the application configuration action.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="useExpression"></param>
		public static void Log(this IApplicationInitializationContext context,
			Expression<Func<IServiceProvider, IServiceProvider>> useExpression)
		{
			Guard.Against.Null(context, nameof(context));
			Guard.Against.Null(useExpression, nameof(useExpression));

			MethodCallExpression methodCallExpression = (useExpression.Body as MethodCallExpression)!;
			Guard.Against.Null(methodCallExpression, nameof(methodCallExpression));

			string methodName = methodCallExpression.Method.Name;
			context.Logger.LogDebug($"Configure: {methodName}");

			ExecuteTryCatch(context.Logger, () =>
			{
				useExpression.Compile().Invoke(context.ServiceProvider);
			});
		}

		/// <summary>
		///     Logs the application configuration action.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="useExpression"></param>
		public static void Log(this IApplicationInitializationContext context,
			Expression<Action<IServiceProvider>> useExpression)
		{
			Guard.Against.Null(context, nameof(context));
			Guard.Against.Null(useExpression, nameof(useExpression));

			MethodCallExpression methodCallExpression = (useExpression.Body as MethodCallExpression)!;
			Guard.Against.Null(methodCallExpression, nameof(methodCallExpression));

			string methodName = methodCallExpression.Method.Name;
			context.Logger.LogDebug($"Configure: {methodName}");

			ExecuteTryCatch(context.Logger, () =>
			{
				useExpression.Compile().Invoke(context.ServiceProvider);
			});
		}

		/// <summary>
		///     Logs the application configuration action.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="methodName"></param>
		/// <param name="useFunction"></param>
		public static void Log(this IApplicationInitializationContext context,
			string methodName,
			Action<IServiceProvider> useFunction)
		{
			Guard.Against.Null(context, nameof(context));
			Guard.Against.Null(useFunction, nameof(useFunction));

			context.Logger.LogDebug($"Configure: {methodName}");

			ExecuteTryCatch(context.Logger, () =>
			{
				useFunction.Invoke(context.ServiceProvider);
			});
		}

		private static void ExecuteTryCatch(ILogger logger, Action action)
		{
			try
			{
				action.Invoke();
			}
			catch(Exception ex)
			{
				logger.LogCritical(ex, ex.Message);
				throw;
			}
		}
	}
}
